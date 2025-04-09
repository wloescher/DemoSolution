import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useState } from "react";
import Spinner from 'react-bootstrap/Spinner';
import { useAuth } from "../utils/AuthProvider";

const Login = () => {
    const { login } = useAuth();
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [isAuthenticating, setIsAuthenticating] = useState(false);

    const handleSubmit = async (event) => {
        event.preventDefault();
        setIsAuthenticating(true);

        // Replace this with your actual data fetching logic
        let dummyToken = {
            NameIdentifier: 0,
            Name: 'admin',
            GivenName: 'test',
            Surname: 'admin',
            Role: 'admin',
        }
        login(dummyToken);
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
                    <div className="row">
                        <div className="col-6">
                            <input id="isActive" type="checkbox" className="form-check-input me-2" aria-label="Active" />
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
                </div>
            </div>
        </form>
    );
}

export default Login;