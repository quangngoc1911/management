'use client';

import { useEffect, useRef, ReactNode } from 'react';
import { X } from 'lucide-react';
import { useTranslation } from '@/shared/i18n/useTranslation';

interface ModalProps {
  open: boolean;
  onClose: () => void;
  title?: string;
  children: ReactNode;
  size?: 'sm' | 'md' | 'lg' | 'xl';
  footer?: ReactNode;
}

const sizeClasses = {
  sm: 'max-w-md',
  md: 'max-w-lg',
  lg: 'max-w-2xl',
  xl: 'max-w-4xl',
};

export function Modal({ 
  open, 
  onClose, 
  title, 
  children, 
  size = 'md',
  footer 
}: ModalProps) {
  const overlayRef = useRef<HTMLDivElement>(null);

  // Handle escape key
  useEffect(() => {
    const handleEscape = (e: KeyboardEvent) => {
      if (e.key === 'Escape' && open) {
        onClose();
      }
    };
    document.addEventListener('keydown', handleEscape);
    return () => document.removeEventListener('keydown', handleEscape);
  }, [open, onClose]);

  // Prevent scroll when modal is open
  useEffect(() => {
    if (open) {
      document.body.style.overflow = 'hidden';
    } else {
      document.body.style.overflow = '';
    }
    return () => {
      document.body.style.overflow = '';
    };
  }, [open]);

  if (!open) return null;

  return (
    <div 
      ref={overlayRef}
      className="fixed inset-0 z-modal flex items-center justify-center p-4"
      onClick={(e) => {
        if (e.target === overlayRef.current) {
          onClose();
        }
      }}
    >
      {/* Overlay */}
      <div className="absolute inset-0 bg-black/50 animate-fade-in" />
      
      {/* Modal */}
      <div className={`relative w-full ${sizeClasses[size]} bg-surface rounded-lg shadow-modal animate-scale-in`}>
        {/* Header */}
        {title && (
          <div className="flex items-center justify-between px-6 py-4 border-b border-border">
            <h2 className="text-lg font-semibold text-foreground">{title}</h2>
            <button
              onClick={onClose}
              className="p-1 rounded-md text-muted hover:text-foreground hover:bg-surface-alt transition"
            >
              <X className="w-5 h-5" />
            </button>
          </div>
        )}

        {/* Body */}
        <div className="px-6 py-4 max-h-[70vh] overflow-y-auto">
          {children}
        </div>

        {/* Footer */}
        {footer && (
          <div className="flex items-center justify-end gap-3 px-6 py-4 border-t border-border">
            {footer}
          </div>
        )}
      </div>
    </div>
  );
}

export function ConfirmModal({
  open,
  onClose,
  title,
  message,
  confirmText,
  cancelText,
  onConfirm,
  danger = false,
}: {
  open: boolean;
  onClose: () => void;
  title?: string;
  message: string;
  confirmText?: string;
  cancelText?: string;
  onConfirm: () => void;
  danger?: boolean;
}) {
  const { t } = useTranslation('common');
  return (
    <Modal open={open} onClose={onClose} title={title ?? t('confirm.title')} size="sm">
      <p className="text-muted">{message}</p>
      <div className="flex justify-end gap-3 mt-6">
        <button
          onClick={onClose}
          className="px-4 py-2 text-sm font-medium text-muted bg-surface-alt rounded-md hover:bg-border transition"
        >
          {cancelText ?? t('actions.cancel')}
        </button>
        <button
          onClick={() => {
            onConfirm();
            onClose();
          }}
          className={`px-4 py-2 text-sm font-medium text-white rounded-md transition ${
            danger 
              ? 'bg-danger hover:bg-danger-hover' 
              : 'bg-primary hover:bg-primary-hover'
          }`}
        >
          {confirmText ?? t('actions.confirm')}
        </button>
      </div>
    </Modal>
  );
}
