import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useParams } from 'react-router-dom';

const AccessDenied = () => {
    const params = useParams();
    const page = params.page;

    return (
        <div className="error">
            <h1>
                <FontAwesomeIcon icon="fa-solid fa-lock" className="me-2" />
                Access Denied
            </h1>
            <p>Sorry, but you don't have permission to access this page.</p>
            <code>{page}</code>
        </div>
    );
}

export default AccessDenied;