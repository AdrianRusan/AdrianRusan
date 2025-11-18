'use client'

import { useState } from 'react'

interface FABMenuItem {
  icon: string
  label: string
  onClick: () => void
}

interface FloatingActionButtonProps {
  items: FABMenuItem[]
}

export default function FloatingActionButton({ items }: FloatingActionButtonProps) {
  const [isOpen, setIsOpen] = useState(false)

  return (
    <div className="fixed bottom-6 right-6 z-40">
      {/* Menu Items */}
      {isOpen && (
        <div className="absolute bottom-16 right-0 flex flex-col space-y-3 mb-2">
          {items.map((item, index) => (
            <button
              key={index}
              onClick={() => {
                item.onClick()
                setIsOpen(false)
              }}
              className="flex items-center space-x-3 bg-white rounded-full shadow-lg pl-4 pr-5 py-3 hover:shadow-xl transition-all transform hover:scale-105 group"
            >
              <span className="text-2xl">{item.icon}</span>
              <span className="text-sm font-medium text-gray-700 whitespace-nowrap group-hover:text-primary-600">
                {item.label}
              </span>
            </button>
          ))}
        </div>
      )}

      {/* Main FAB Button */}
      <button
        onClick={() => setIsOpen(!isOpen)}
        className={`w-14 h-14 rounded-full shadow-lg flex items-center justify-center transition-all transform ${
          isOpen
            ? 'bg-red-500 hover:bg-red-600 rotate-45'
            : 'bg-primary-600 hover:bg-primary-700 hover:scale-110'
        }`}
        aria-label={isOpen ? 'Close menu' : 'Open menu'}
      >
        <svg
          className="w-6 h-6 text-white"
          fill="none"
          strokeLinecap="round"
          strokeLinejoin="round"
          strokeWidth="2"
          viewBox="0 0 24 24"
          stroke="currentColor"
        >
          <path d="M12 4v16m8-8H4" />
        </svg>
      </button>
    </div>
  )
}
