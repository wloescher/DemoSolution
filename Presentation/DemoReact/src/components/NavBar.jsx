import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import './NavBar.jsx.css';

function NavBar() {
    const navigate = useNavigate();
    const [searchText, setSearchText] = useState('');

    function searchSubmit() {
        // Check for empty value
        if (!searchText) return;

        // Check for search page
        if (window.location.pathname == '/search') {
            // Force update of querystring values
            const url = new URL(location.href);
            url.searchParams.set('text', searchText);
            location.assign(url.search);
        }
        else {
            // Load search page
            navigate({
                pathname: '/search',
                search: '?text=' + encodeURIComponent(searchText)
            });
        }

        // Clear value
        setSearchText('');
    }

    return (
        <nav className="navbar fixed-top navbar-expand-lg bg-body-tertiary">
            <div className="container-fluid">
                <a href="/" className="navbar-brand">
                    <FontAwesomeIcon icon="fa-solid fa-code" className="me-2" />
                    DemoAngular
                </a>
                <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span className="navbar-toggler-icon"></span>
                </button>
                <div className="collapse navbar-collapse" id="navbarSupportedContent">
                    <ul className="navbar-nav me-auto mb-2 mb-lg-0">
                        <li className="nav-item dropdown">
                            <a className="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                                Clients
                            </a>
                            <ul className="dropdown-menu">
                                <li><a className="dropdown-item" href="/clients">ALL</a></li>
                                <li><hr className="dropdown-divider" /></li>
                                <li>
                                    <a className="dropdown-item" href="/clients/internal">Internal</a>
                                </li>
                                <li>
                                    <a className="dropdown-item" href="/clients/external">External</a>
                                </li>
                                <li>
                                    <a className="dropdown-item" href="/clients/lead">Lead</a>
                                </li>
                            </ul>
                        </li>
                        <li className="nav-item dropdown">
                            <a className="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                                Users
                            </a>
                            <ul className="dropdown-menu">
                                <li><a className="dropdown-item" href="/users">ALL</a></li>
                                <li><hr className="dropdown-divider" /></li>
                                <li>
                                    <a className="dropdown-item" href="/users/admin">Admin</a>
                                </li>
                                <li>
                                    <a className="dropdown-item" href="/users/client">Client</a>
                                </li>
                                <li>
                                    <a className="dropdown-item" href="/users/sales">Sales</a>
                                </li>
                                <li>
                                    <a className="dropdown-item" href="/users/marketing">Marketing</a>
                                </li>
                                <li>
                                    <a className="dropdown-item" href="/users/accounting">Accounting</a>
                                </li>
                                <li>
                                    <a className="dropdown-item" href="/users/executive">Executive</a>
                                </li>
                            </ul>
                        </li>
                        <li className="nav-item dropdown">
                            <a className="nav-link dropdown-toggle"
                                href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                                Work Items
                            </a>
                            <ul className="dropdown-menu">
                                <li><a className="dropdown-item" href="/workitems">ALL</a></li>
                                <li><hr className="dropdown-divider" /></li>
                                <li>
                                    <a className="dropdown-item" href="/workitems/user-story">User Story</a>
                                </li>
                                <li>
                                    <a className="dropdown-item" href="/workitems/task">Task</a>
                                </li>
                                <li>
                                    <a className="dropdown-item" href="/workitems/bug">Bug</a>
                                </li>
                                <li>
                                    <a className="dropdown-item" href="/workitems/epic">Epic</a>
                                </li>
                                <li>
                                    <a className="dropdown-item" href="/workitems/feature">Feature</a>
                                </li>
                            </ul>
                        </li>
                    </ul>
                    <form className="d-flex btn-group" role="search" onSubmit={searchSubmit}>
                        <input className="form-control border" type="search" placeholder="Search..." aria-label="Search" value={searchText} onChange={(e) => setSearchText(e.target.value)} />
                        <button className="btn btn-outline-success" type="submit">
                            <FontAwesomeIcon icon="fa-solid fa-search" />
                        </button>
                    </form>
                </div>
            </div>
        </nav >
    );
}

export default NavBar;