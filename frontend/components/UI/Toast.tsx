'use client'

import { useEffect } from 'react'

export interface ToastProps {
  type?: 'success' | 'error' | 'warning' | 'info'
  message: string
  duration?: number
  onClose: () => void
}

export default function Toast({ type = 'info', message, duration = 3000, onClose }: ToastProps) {
  useEffect(() => {
    const timer = setTimeout(() => {
      onClose()
    }, duration)

    return () => clearTimeout(timer)
  }, [duration, onClose])

  const colors = {
    success: 'bg-green-600',
    error: 'bg-red-600',
    warning: 'bg-yellow-600',
    info: 'bg-blue-600',
  }

  const icons = {
    success: '✓',
    error: '✕',
    warning: '⚠',
    info: 'ℹ',
  }

  return (
    <div
      className={`fixed top-20 right-4 z-50 ${colors[type]} text-white px-6 py-4 rounded-lg shadow-2xl flex items-center space-x-3 animate-slide-in-right max-w-sm`}
      role="alert"
    >
      <div className="flex-shrink-0 w-6 h-6 rounded-full bg-white bg-opacity-30 flex items-center justify-center font-bold">
        {icons[type]}
      </div>
      <p className="font-medium">{message}</p>
      <button
        onClick={onClose}
        className="ml-4 p-1 hover:bg-white hover:bg-opacity-20 rounded transition-colors"
        aria-label="Close"
      >
        <svg className="w-4 h-4" fill="none" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" viewBox="0 0 24 24" stroke="currentColor">
          <path d="M6 18L18 6M6 6l12 12" />
        </svg>
      </button>
    </div>
  )
}
