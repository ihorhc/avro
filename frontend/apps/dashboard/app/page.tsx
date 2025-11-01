import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@avro/ui'

export default function Home() {
  return (
    <main className="container mx-auto p-8">
      <div className="mb-8">
        <h1 className="text-4xl font-bold mb-2">AVRO Platform Dashboard</h1>
        <p className="text-muted-foreground">
          AI-First Monorepo SDLC Platform - Enterprise-scale AI-driven development lifecycle
        </p>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 mb-8">
        <Card>
          <CardHeader>
            <CardTitle>Projects</CardTitle>
            <CardDescription>100+ interconnected projects</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="text-3xl font-bold">127</div>
            <p className="text-sm text-muted-foreground mt-2">
              98 active, 29 archived
            </p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Tasks</CardTitle>
            <CardDescription>Structured task management</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="text-3xl font-bold">5,247</div>
            <p className="text-sm text-muted-foreground mt-2">
              3,891 completed, 756 in progress
            </p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>AI Workflows</CardTitle>
            <CardDescription>Autonomous automation</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="text-3xl font-bold">4</div>
            <p className="text-sm text-muted-foreground mt-2">
              Issue-to-PR, Code Review, Task Breakdown, Dependencies
            </p>
          </CardContent>
        </Card>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <Card>
          <CardHeader>
            <CardTitle>Recent Activity</CardTitle>
            <CardDescription>Latest platform updates</CardDescription>
          </CardHeader>
          <CardContent>
            <ul className="space-y-2">
              <li className="flex items-start gap-2">
                <span className="text-green-500 mt-1">✓</span>
                <div>
                  <p className="font-medium">TASK-0005 completed</p>
                  <p className="text-sm text-muted-foreground">
                    Implemented shadcn UI component library
                  </p>
                </div>
              </li>
              <li className="flex items-start gap-2">
                <span className="text-blue-500 mt-1">⟳</span>
                <div>
                  <p className="font-medium">TASK-0004 in progress</p>
                  <p className="text-sm text-muted-foreground">
                    Create AI agent workflow engine
                  </p>
                </div>
              </li>
              <li className="flex items-start gap-2">
                <span className="text-green-500 mt-1">✓</span>
                <div>
                  <p className="font-medium">TASK-0003 completed</p>
                  <p className="text-sm text-muted-foreground">
                    Set up project dependency graph
                  </p>
                </div>
              </li>
            </ul>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>System Health</CardTitle>
            <CardDescription>Platform status overview</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              <div>
                <div className="flex justify-between mb-1">
                  <span className="text-sm font-medium">Backend Services</span>
                  <span className="text-sm text-green-500">Operational</span>
                </div>
                <div className="w-full bg-secondary rounded-full h-2">
                  <div className="bg-green-500 h-2 rounded-full" style={{ width: '100%' }}></div>
                </div>
              </div>
              <div>
                <div className="flex justify-between mb-1">
                  <span className="text-sm font-medium">Frontend Apps</span>
                  <span className="text-sm text-green-500">Operational</span>
                </div>
                <div className="w-full bg-secondary rounded-full h-2">
                  <div className="bg-green-500 h-2 rounded-full" style={{ width: '100%' }}></div>
                </div>
              </div>
              <div>
                <div className="flex justify-between mb-1">
                  <span className="text-sm font-medium">AI Workflows</span>
                  <span className="text-sm text-green-500">Active</span>
                </div>
                <div className="w-full bg-secondary rounded-full h-2">
                  <div className="bg-green-500 h-2 rounded-full" style={{ width: '100%' }}></div>
                </div>
              </div>
            </div>
          </CardContent>
        </Card>
      </div>

      <div className="mt-8">
        <Card>
          <CardHeader>
            <CardTitle>Quick Links</CardTitle>
            <CardDescription>Navigate to key resources</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
              <a href="/docs" className="p-4 border rounded-lg hover:bg-accent transition-colors">
                <div className="font-medium">Documentation</div>
                <div className="text-sm text-muted-foreground">Guides & APIs</div>
              </a>
              <a href="/tasks" className="p-4 border rounded-lg hover:bg-accent transition-colors">
                <div className="font-medium">Task Registry</div>
                <div className="text-sm text-muted-foreground">Manage tasks</div>
              </a>
              <a href="/projects" className="p-4 border rounded-lg hover:bg-accent transition-colors">
                <div className="font-medium">Projects</div>
                <div className="text-sm text-muted-foreground">View projects</div>
              </a>
              <a href="/workflows" className="p-4 border rounded-lg hover:bg-accent transition-colors">
                <div className="font-medium">Workflows</div>
                <div className="text-sm text-muted-foreground">AI automation</div>
              </a>
            </div>
          </CardContent>
        </Card>
      </div>
    </main>
  )
}
