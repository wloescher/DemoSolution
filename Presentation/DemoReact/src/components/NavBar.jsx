import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import './NavBar.jsx.css';

function NavBar() {
    const navigate = useNavigate();
    const [searchText, setSearchText] = useState('');

    function searchSubmit() {
        alert('TODO: Implement search');
        return;

        //// Check for empty value
        //if (!searchText) return;

        //// Check for search page
        //if (window.location.pathname == '/search') {
        //    // Force update of querystring values
        //    const url = new URL(location.href);
        //    url.searchParams.set('text', searchText);
        //    location.assign(url.search);
        //}
        //else {
        //    // Load search page
        //    navigate({
        //        pathname: '/search',
        //        search: '?text=' + encodeURIComponent(searchText)
        //    });
        //}

        //// Clear value
        //setSearchText('');
    }

    return (
        <nav className="navbar fixed-top navbar-expand-lg bg-body-tertiary">
            <div className="container-fluid">
                <Link to="/" className="navbar-brand">
                    <FontAwesomeIcon icon="fa-brands fa-react" className="me-2" />
                    DemoAngular
                </Link>
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
                                <li>
                                    <Link to="/clients" className="dropdown-item">ALL</Link>
                                </li>
                                <li>
                                    <hr className="dropdown-divider" />
                                </li>
                                <li>
                                    <Link to="/clients/internal" className="dropdown-item">Internal</Link>
                                </li>
                                <li>
                                    <Link to="/clients/external" className="dropdown-item">External</Link>
                                </li>
                                <li>
                                    <Link to="/clients/lead" className="dropdown-item">Lead</Link>
                                </li>
                            </ul>
                        </li>
                        <li className="nav-item dropdown">
                            <a className="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                                Users
                            </a>
                            <ul className="dropdown-menu">
                                <li>
                                    <Link to="/users" className="dropdown-item">ALL</Link>
                                </li>
                                <li>
                                    <hr className="dropdown-divider" />
                                </li>
                                <li>
                                    <Link to="/users/admin" className="dropdown-item">Admin</Link>
                                </li>
                                <li>
                                    <Link to="/users/client" className="dropdown-item">Client</Link>
                                </li>
                                <li>
                                    <Link to="/users/sales" className="dropdown-item">Sales</Link>
                                </li>
                                <li>
                                    <Link to="/users/marketing" className="dropdown-item">Marketing</Link>
                                </li>
                                <li>
                                    <Link to="/users/accounting" className="dropdown-item">Accounting</Link>
                                </li>
                                <li>
                                    <Link to="/users/executive" className="dropdown-item">Executive</Link>
                                </li>
                            </ul>
                        </li>
                        <li className="nav-item dropdown">
                            <a className="nav-link dropdown-toggle"
                                href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                                Work Items
                            </a>
                            <ul className="dropdown-menu">
                                <li>
                                    <Link to="/workitems" className="dropdown-item">ALL</Link>
                                </li>
                                <li>
                                    <hr className="dropdown-divider" />
                                </li>
                                <li>
                                    <Link to="/workitems/user-story" className="dropdown-item">User Story</Link>
                                </li>
                                <li>
                                    <Link to="/workitems/task" className="dropdown-item">Task</Link>
                                </li>
                                <li>
                                    <Link to="/workitems/bug" className="dropdown-item">Bug</Link>
                                </li>
                                <li>
                                    <Link to="/workitems/epic" className="dropdown-item">Epic</Link>
                                </li>
                                <li>
                                    <Link to="/workitems/feature" className="dropdown-item">Feature</Link>
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