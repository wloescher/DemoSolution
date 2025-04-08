import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useState } from "react";
import UserDataGrid from "../user/UserDataGrid"
import { useLocation, Link } from 'react-router-dom';

function UserList() {
    const [isUsersLoading, setIsUsersLoading] = useState();
    const [usersRecordCount, setUsersRecordCount] = useState();
    const [usersGridRef, setUsersGridRef] = useState();
    const location = useLocation();

    const getButtonClassName = (path) => {
        var className = 'btn btn-outline-secondary';
        if (location.pathname === path) {
            className += ' bg-info';
        }
        return className;
    };

    function onUsersGridReady(params) {
        setUsersGridRef(params.api);
    }

    return (
        <>
            <div className="container">
                <div className="row">
                    <div className="col">
                        <h1>User List</h1>
                    </div>
                    <div className="col text-end">
                        <div className="btn-group border" role="group" >
                            <Link to="/users" className={getButtonClassName('/users')}>ALL</Link>
                            <Link to="/users/admin" className={getButtonClassName('/users/admin')}>Admin</Link>
                            <Link to="/users/client" className={getButtonClassName('/users/client')}>Client</Link>
                            <Link to="/users/sales" className={getButtonClassName('/users/sales')}>Sales</Link>
                            <Link to="/users/marketing" className={getButtonClassName('/users/marketing')}>Marketing</Link>
                            <Link to="/users/accounting" className={getButtonClassName('/users/accounting')}>Accounting</Link>
                            <Link to="/users/executives" className={getButtonClassName('/users/executives')}>Executives</Link>
                        </div>
                    </div>
                    <div className="col-1 text-end">
                        <Link to="/user/add" className="btn btn-primary">
                            <FontAwesomeIcon icon="fa-solid fa-plus" className="me-2" /> Add
                        </Link>
                    </div>
                </div>
                <div className="data-grid">
                    <UserDataGrid isLoading={isUsersLoading}
                        setIsLoading={setIsUsersLoading}
                        recordCount={usersRecordCount}
                        setRecordCount={setUsersRecordCount}
                        onGridReady={onUsersGridReady} />
                </div>
            </div>
        </>

    );
}

export default UserList;