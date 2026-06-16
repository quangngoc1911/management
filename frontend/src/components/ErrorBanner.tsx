import { AlertCircle } from 'lucide-react';

/** Inline danger banner. Renders nothing when there is no message. */
export function ErrorBanner({ message }: { message?: string | null }) {
    if (!message) return null;
    return (
        <div className="flex items-center gap-2 p-4 rounded-md bg-danger/10 text-danger text-sm">
            <AlertCircle className="w-4 h-4 shrink-0" />
            <span>{message}</span>
        </div>
    );
}

export default ErrorBanner;
