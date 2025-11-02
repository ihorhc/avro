#!/bin/bash
# AI Auto-Development Workflow Validation Script
# This script validates the ai-autodev.yml workflow before deployment

set -e

WORKFLOW_FILE=".github/workflows/ai-autodev.yml"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"

cd "${REPO_ROOT}"

echo "🔍 Validating AI Auto-Development Workflow..."
echo ""

# Check if workflow file exists
if [[ ! -f "${WORKFLOW_FILE}" ]]; then
    echo "❌ ERROR: Workflow file not found: ${WORKFLOW_FILE}"
    exit 1
fi

echo "✅ Workflow file exists: ${WORKFLOW_FILE}"
echo ""

# Validate YAML syntax
echo "📋 Validating YAML syntax..."
if command -v yamllint &> /dev/null; then
    # Check for errors only (warnings are acceptable)
    if yamllint "${WORKFLOW_FILE}" 2>&1 | grep -q "::error"; then
        echo "❌ YAML syntax errors found:"
        yamllint "${WORKFLOW_FILE}" 2>&1 | grep "::error"
        exit 1
    else
        echo "✅ YAML syntax is valid (warnings may exist)"
    fi
else
    echo "⚠️  yamllint not found, skipping YAML validation"
    echo "   Install with: pip install yamllint"
fi
echo ""

# Validate GitHub Actions workflow
echo "🔧 Validating GitHub Actions workflow..."
if command -v actionlint &> /dev/null; then
    if actionlint "${WORKFLOW_FILE}"; then
        echo "✅ Workflow validation passed"
    else
        echo "❌ Workflow validation failed"
        exit 1
    fi
else
    echo "⚠️  actionlint not found, skipping workflow validation"
    echo "   Install from: https://github.com/rhysd/actionlint"
fi
echo ""

# Validate workflow structure with Python
echo "🐍 Validating workflow structure..."
python3 << 'EOF'
import yaml
import sys

try:
    with open('.github/workflows/ai-autodev.yml', 'r') as f:
        workflow = yaml.safe_load(f)
    
    errors = []
    warnings = []
    
    # Check job names for hyphens
    jobs = workflow.get('jobs', {})
    for job_name in jobs.keys():
        if '-' in job_name:
            errors.append(f"Job name '{job_name}' contains hyphen (use underscore)")
    
    # Check required jobs exist
    required_jobs = [
        'check_trigger',
        'analyze_architecture',
        'parallel_agents',
        'code_review',
        'create_pull_request'
    ]
    for required_job in required_jobs:
        if required_job not in jobs:
            errors.append(f"Required job '{required_job}' is missing")
    
    # Check outputs are defined
    required_outputs = {
        'check_trigger': ['should_process', 'issue_number', 'issue_title'],
        'analyze_architecture': ['branch_name', 'branch_created'],
        'create_pull_request': ['pr_number']
    }
    
    for job_name, expected_outputs in required_outputs.items():
        if job_name in jobs:
            job_outputs = jobs[job_name].get('outputs', {})
            for expected_output in expected_outputs:
                if expected_output not in job_outputs:
                    warnings.append(
                        f"Job '{job_name}' missing expected output '{expected_output}'"
                    )
    
    # Check needs dependencies
    for job_name, job_config in jobs.items():
        needs = job_config.get('needs', [])
        if isinstance(needs, str):
            needs = [needs]
        for need in needs:
            if '-' in need:
                errors.append(
                    f"Job '{job_name}' references dependency '{need}' "
                    f"with hyphen (should use underscore)"
                )
            if need not in jobs:
                errors.append(
                    f"Job '{job_name}' depends on non-existent job '{need}'"
                )
    
    # Print results
    if errors:
        print("❌ Validation errors found:")
        for error in errors:
            print(f"  - {error}")
        sys.exit(1)
    
    if warnings:
        print("⚠️  Warnings:")
        for warning in warnings:
            print(f"  - {warning}")
    
    print("✅ Workflow structure is valid")
    
except Exception as e:
    print(f"❌ Failed to validate workflow: {e}")
    sys.exit(1)
EOF

if [[ $? -ne 0 ]]; then
    echo ""
    echo "❌ Workflow structure validation failed"
    exit 1
fi
echo ""

# Check documentation exists
echo "📚 Checking documentation..."
DOC_FILE="docs/automation/AI_AUTODEV_WORKFLOW.md"
if [[ -f "${DOC_FILE}" ]]; then
    echo "✅ Documentation exists: ${DOC_FILE}"
    
    # Check for key sections
    REQUIRED_SECTIONS=(
        "Prerequisites"
        "Required Secrets"
        "Usage"
        "Troubleshooting"
    )
    
    for section in "${REQUIRED_SECTIONS[@]}"; do
        if grep -q "${section}" "${DOC_FILE}"; then
            echo "  ✅ Section found: ${section}"
        else
            echo "  ⚠️  Section missing: ${section}"
        fi
    done
else
    echo "⚠️  Documentation not found: ${DOC_FILE}"
fi
echo ""

# Summary
echo "================================================"
echo "✅ All validation checks passed!"
echo "================================================"
echo ""
echo "The workflow is ready for deployment."
echo ""
echo "Next steps:"
echo "1. Test in a development branch with a test issue"
echo "2. Monitor workflow execution in GitHub Actions"
echo "3. Review generated PRs and validate outputs"
echo "4. Merge to main when confirmed working"
