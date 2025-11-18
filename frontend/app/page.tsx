import Link from 'next/link'

export default function Home() {
  return (
    <main className="min-h-screen p-8">
      <div className="max-w-7xl mx-auto">
        <header className="mb-12">
          <h1 className="text-4xl font-bold text-gray-900 mb-4">
            PUG Platform
          </h1>
          <p className="text-xl text-gray-600">
            Local Council Urban Planning Management System
          </p>
        </header>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <Card
            title="Map Viewer"
            description="View parcels and spatial data on an interactive map"
            href="/map"
            icon="🗺️"
          />
          <Card
            title="Submit Application"
            description="Submit tree cutting, demolition, or construction applications"
            href="/applications/new"
            icon="📝"
          />
          <Card
            title="Report Issue"
            description="Report infrastructure or urban issues"
            href="/issues/new"
            icon="🚧"
          />
          <Card
            title="Marketplace"
            description="Browse available land parcels for sale"
            href="/marketplace"
            icon="🏘️"
          />
          <Card
            title="My Applications"
            description="Track your submitted applications"
            href="/applications/my"
            icon="📋"
          />
          <Card
            title="Admin Dashboard"
            description="Manage applications and system data (staff only)"
            href="/admin"
            icon="⚙️"
          />
        </div>

        <div className="mt-12 p-6 bg-blue-50 rounded-lg">
          <h2 className="text-2xl font-semibold mb-4">Getting Started</h2>
          <ul className="space-y-2 text-gray-700">
            <li>• View the map to explore parcels and their details</li>
            <li>• Register an account to submit applications</li>
            <li>• Report issues without requiring an account</li>
            <li>• Browse the marketplace for available land</li>
          </ul>
        </div>
      </div>
    </main>
  )
}

function Card({ title, description, href, icon }: {
  title: string
  description: string
  href: string
  icon: string
}) {
  return (
    <Link
      href={href}
      className="block p-6 bg-white rounded-lg shadow-md hover:shadow-lg transition-shadow border border-gray-200"
    >
      <div className="text-4xl mb-4">{icon}</div>
      <h3 className="text-xl font-semibold mb-2 text-gray-900">{title}</h3>
      <p className="text-gray-600">{description}</p>
    </Link>
  )
}
