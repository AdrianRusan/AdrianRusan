'use client'

import { ReactNode } from 'react'
import Header from './Header'

interface LayoutProps {
  children: ReactNode
  showHeader?: boolean
}

export default function Layout({ children, showHeader = true }: LayoutProps) {
  return (
    <div className="min-h-screen bg-gray-50">
      {showHeader && <Header />}
      <main className={showHeader ? 'pt-16' : ''}>
        {children}
      </main>
    </div>
  )
}
