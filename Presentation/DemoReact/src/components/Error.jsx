function Error(error) {
    return (
        <div className="error">
            <h1><i className="fa fa-exclamation-triangle"></i> {error.title}</h1>
            <p>{error.message}</p>
            <code>{error.details}</code>
            <p>{error.url}</p>
        </div>);
}

export default Error;