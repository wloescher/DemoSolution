import React from "react";

class ErrorBoundary extends React.Component {
    state = { hasError: false };

    static getDerivedStateFromError() {
        return { hasError: true };
    };

    componentDidCatch(error, info) {
        console.log(error, info);
    }

    render() {
        if (this.state.hasError) {
            return <h4>{this.props.fallback}</h4>;
        }
        return this.props.children;
    }
}

export default ErrorBoundary;