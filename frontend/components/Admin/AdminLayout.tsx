'use client'

import { useState, ReactNode } from 'react'
import Link from 'next/link'
import { usePathname } from 'next/navigation'

interface AdminLayoutProps {
  children: ReactNode
}

export default function AdminLayout({ children }: AdminLayoutProps) {
  const [isSidebarOpen, setIsSidebarOpen] = useState(false)
  const pathname = usePathname()

  const menuItems = [
    { href: '/admin', icon: '📊', label: 'Dashboard' },
    { href: '/admin/applications', icon: '📝', label: 'Applications' },
    { href: '/admin/issues', icon: '🚧', label: 'Issues' },
    { href: '/admin/sales', icon: '🏘️', label: 'Sales' },
    { href: '/admin/scans', icon: '📐', label: '3D Scans' },
    { href: '/admin/taxes', icon: '💰', label: 'Taxes' },
    { href: '/admin/users', icon: '👥', label: 'Users' },
    { href: '/admin/settings', icon: '⚙️', label: 'Settings' },
  ]

  return (
    <div className="min-h-screen bg-gray-100">
      {/* Mobile Sidebar Overlay */}
      {isSidebarOpen && (
        <div
          className="fixed inset-0 bg-black bg-opacity-50 z-40 lg:hidden"
          onClick={() => setIsSidebarOpen(false)}
        />
      )}

      {/* Sidebar */}
      <aside
        className={`fixed top-16 left-0 bottom-0 w-64 bg-white shadow-lg transform transition-transform duration-300 ease-in-out z-50 ${
          isSidebarOpen ? 'translate-x-0' : '-translate-x-full lg:translate-x-0'
        }`}
      >
        <nav className="h-full overflow-y-auto py-6">
          <div className="px-4 mb-6">
            <h2 className="text-xs font-semibold text-gray-500 uppercase tracking-wide">
              Admin Panel
            </h2>
          </div>
          <ul className="space-y-1 px-3">
            {menuItems.map((item) => (
              <li key={item.href}>
                <Link
                  href={item.href}
                  className={`flex items-center space-x-3 px-4 py-3 rounded-lg transition-colors ${
                    pathname === item.href
                      ? 'bg-primary-100 text-primary-700'
                      : 'text-gray-700 hover:bg-gray-100'
                  }`}
                  onClick={() => setIsSidebarOpen(false)}
                >
                  <span className="text-xl">{item.icon}</span>
                  <span className="font-medium text-sm">{item.label}</span>
                </Link>
              </li>
            ))}
          </ul>
        </nav>
      </aside>

      {/* Main Content */}
      <div className="lg:pl-64 pt-16">
        {/* Mobile Menu Button */}
        <div className="lg:hidden fixed top-20 left-4 z-30">
          <button
            onClick={() => setIsSidebarOpen(true)}
            className="p-2 bg-white rounded-lg shadow-md text-gray-700 hover:bg-gray-50"
            aria-label="Open menu"
          >
            <svg className="w-6 h-6" fill="none" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" viewBox="0 0 24 24" stroke="currentColor">
              <path d="M4 6h16M4 12h16M4 18h16" />
            </svg>
          </button>
        </div>

        {/* Page Content */}
        <div className="p-6">
          {children}
        </div>
      </div>
    </div>
  )
}
