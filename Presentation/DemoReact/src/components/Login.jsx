import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import Spinner from 'react-bootstrap/Spinner';
import { useAuth } from "../utils/AuthProvider";

const Login = () => {
    const navigate = useNavigate();
    const { login } = useAuth();
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [rememberMe, setRememberMe] = useState('');
    const [isAuthenticating, setIsAuthenticating] = useState(false);
    const [authenticedFailed, setAuthenticationFailed] = useState(false);

    const handleSubmit = async (event) => {
        event.preventDefault();

        setIsAuthenticating(true);
        setAuthenticationFailed(false);

        const isAuthenticated = login(username, password, rememberMe);
        if (isAuthenticated) {
            navigate('/');
        } else {
            setIsAuthenticating(false);
            setAuthenticationFailed(true);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <div className="row">
                <div className="col-2 offset-md-5">
                    <h1 className="text-center">
                        <FontAwesomeIcon icon="fa-brands fa-react" className="me-2" />
                        DemoReact
                    </h1>
                    <input id="username" type="text" className="form-control required mb-2" required
                        aria-label="Username" placeholder="Username"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)} />
                    <input id="password" type="password" className="form-control required mb-2" required
                        aria-label="Password" placeholder="Password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)} />
                    <div className="row ">
                        <div className="col-6 pt-1">
                            <input id="rememberMe" type="checkbox" className="form-check-input me-2" aria-label="Remember Me"
                                value={rememberMe}
                                onChange={(e) => setRememberMe(e.target.checked)} />
                            <label htmlFor="isActive" className="form-label">Remember Me</label>
                        </div>
                        <div className="col-6 text-end">
                            <button type="submit" className="btn btn-primary">
                                <FontAwesomeIcon icon="fa-solid fa-sign-in" className="me-2" />
                                Login
                            </button>
                        </div>
                    </div>
                    {isAuthenticating &&
                        <div className="row">
                            <div className="col text-secondary">
                                <Spinner size="sm" animation="border" role="status" /> Authenticating...
                            </div>
                        </div>}
                    {authenticedFailed &&
                        <div class="alert alert-danger mt-2" role="alert">
                            <FontAwesomeIcon icon="fa-solid fa-exclamation-triangle" className="me-2" />
                            Invalid username or password!
                        </div>}
                </div>
            </div>
        </form>
    );
}

export default Login;