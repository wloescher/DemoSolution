import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Link } from 'react-router-dom';

const Logout = () => {
    return (
        <div className="error">
            <h1>
                <FontAwesomeIcon icon="fa-solid fa-sign-out" className="me-2" />
                Logout
            </h1>
            <p>You have been logged out.</p>
            <Link to="/Login" className="btn btn-primary">
                <FontAwesomeIcon icon="fa-solid fa-sign-in" className="me-2" />
                Login
            </Link>            
        </div>
    );
}

export default Logout;