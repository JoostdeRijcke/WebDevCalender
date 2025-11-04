import React, { useEffect, useRef } from "react";


type Props = {
    isOpen: boolean;
    title?: string;
    onClose: () => void;
    children: React.ReactNode;
    width?: number;
};

const Modal: React.FC<Props> = ({ isOpen, title, onClose, children, width = 400 }) => {
    const modalRef = useRef<HTMLDivElement>(null);


    useEffect(() => {
        if (!isOpen) return;
        const onKey = (e: KeyboardEvent) => {
            if (e.key === "Escape") onClose();
        };
        document.addEventListener("keydown", onKey);
        const prev = document.activeElement as HTMLElement | null;
        modalRef.current?.focus();
        return () => {
            document.removeEventListener("keydown", onKey);
            prev?.focus?.();
        };
    }, [isOpen, onClose]);


    if (!isOpen) return null;
    return (
        <div
            role="dialog"
            aria-modal="true"
            aria-label={title}
            onClick={onClose}
            style={{
                position: "fixed",
                inset: 0,
                background: "rgba(0,0,0,0.4)",
                display: "flex",
                alignItems: "flex-start",
                justifyContent: "center",
                paddingTop: "10vh",
                zIndex: 1000,
            }}
        >
            <div
                ref={modalRef}
                tabIndex={-1}
                onClick={(e) => e.stopPropagation()}
                style={{
                    width,
                    maxWidth: "90vw",
                    background: "#fff",
                    border: "1px solid #ccc",
                    borderRadius: 8,
                    padding: 16,
                    boxShadow: "0 8px 24px rgba(0,0,0,0.2)",
                }}

            >
                {title && <h2 style={{ marginTop: 0 }}>{title}</h2>}
                {children}
                <div style={{ display: "flex", justifyContent: "flex-end", marginTop: 12 }}>
                    <button
                        onClick={onClose}
                        style={{
                            padding: "8px 12px",
                            border: "none",
                            borderRadius: 4,
                            background: "#6c757d",
                            color: "#fff",
                            cursor: "pointer",
                        }}
                    >
                        Close
                    </button>
                </div>
            </div>
        </div>
    );
};


export default Modal;