import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

function RouteNotFound() {
    return (
        <div className="error">
            <h1>
                <FontAwesomeIcon icon="fa-solid fa-exclamation-triangle" className="me-2" />
                Route Not Found
            </h1>
            <p>Sorry, the page you are looking for does not exist.</p>
            <code>{ window.location.href }</code>
        </div>
    );
}

export default RouteNotFound;