import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useState } from "react";
import { useParams, Link } from 'react-router-dom';

// Functions
import { useLoadData } from '../functions';
import { getUserTypes } from '../functions';
import { getRegions } from '../functions';
import { getCountries } from '../functions';

const UserEdit = () => {
    const [data = [], setData] = useState();
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState();
    const params = useParams();
    const id = params.id ? parseInt(params.id) : 0;

    const saveChanges = () => {
        alert("TODO: Save changes.");
    }

    const deleteItem = () => {
        alert("TODO: Delete item.");
    }

    // ------------------------------------------------------------
    // Load data
    // ------------------------------------------------------------

    useLoadData('/test/user/' + id, setIsLoading, setData, setError);

    // ------------------------------------------------------------
    // Presentation Layer
    // ------------------------------------------------------------

    return (
        <>
            <div className="container">
                <div className="row">
                    <div className="col">
                        <h1>{id == 0 ? "Add" : "Edit"} User</h1>
                    </div>
                    <div className="col text-end mt-2">
                        <Link to={`/user/${id}`} className="btn btn-light border me-2">
                            <FontAwesomeIcon icon="fa-solid fa-x" className="me-2" /> Cancel
                        </Link>
                        <button type="submit" className="btn btn-primary" onClick={() => saveChanges()}>
                            <FontAwesomeIcon icon="fa-solid fa-save" className="me-2" /> Save
                        </button>
                    </div>
                </div>
                <form>
                    <div className="row mb-2">
                        <div className="col">
                            <div className="mb-3">
                                <label htmlFor="firstName" className="form-label">Name</label>
                                <div className="row">
                                    <div className="col">
                                        <input id="firstName" type="text" className="form-control required" aria-label="First Name" value={data.firstName ?? ''} required placeholder="First Name"
                                            onChange={(e) =>
                                                setData({
                                                    ...data,
                                                    firstName: e.target.value,
                                                })
                                            }
                                        />
                                    </div>
                                    <div className="col">
                                        <input id="middleName" type="text" className="form-control required" aria-label="Middle Name" value={data.middleName ?? ''} required placeholder="Middle Name"
                                            onChange={(e) =>
                                                setData({
                                                    ...data,
                                                    middleName: e.target.value,
                                                })
                                            }
                                        />
                                    </div>
                                    <div className="col">
                                        <input id="lastName" type="text" className="form-control required" aria-label="Last Name" value={data.lastName ?? ''} required placeholder="Last Name"
                                            onChange={(e) =>
                                                setData({
                                                    ...data,
                                                    lastName: e.target.value,
                                                })
                                            }
                                        />
                                    </div>
                                </div>
                            </div>
                            <div className="mb-3">
                                <label htmlFor="typeId" className="form-label">User Type</label>
                                <select id="typeId" className="form-select required" aria-label="Type" value={data.typeId ?? 0} required
                                    onChange={(e) =>
                                        setData({
                                            ...data,
                                            typeId: e.target.value,
                                        })
                                    }
                                >
                                    {getUserTypes().map(item => (
                                        <option key={item.value} value={item.value}>{item.label}</option>
                                    ))}
                                </select>
                            </div>
                            <div className="mb-3">
                                <label htmlFor="phoneNumber" className="form-label">Phone Number</label>
                                <input id="phoneNumber" type="tel" className="form-control" aria-label="Phone Number" value={data.phoneNumber ?? ''}
                                    onChange={(e) =>
                                        setData({
                                            ...data,
                                            phoneNumber: e.target.value,
                                        })
                                    }
                                />
                            </div>
                            <div className="mb-3">
                                <label htmlFor="url" className="form-label">URL / Website</label>
                                <input id="url" type="url" className="form-control" aria-label="URL" value={data.url ?? ''}
                                    onChange={(e) =>
                                        setData({
                                            ...data,
                                            url: e.target.value,
                                        })
                                    }
                                />
                            </div>
                            <div className="mb-3">
                                <input id="isActive" type="checkbox" className="form-check-input me-2" aria-label="Active" checked={data.isActive ?? false}
                                    onChange={(e) =>
                                        setData({
                                            ...data,
                                            isActive: e.target.checked,
                                        })
                                    }
                                />
                                <label htmlFor="isActive" className="form-label">Active</label>
                            </div>
                        </div>
                        <div className="col">
                            <div className="card">
                                <div className="card-header">Address</div>
                                <div className="card-body">
                                    <div className="mb-3">
                                        <div className="mb-3">
                                            <label htmlFor="addressLine1" className="form-label">Address</label>
                                            <input id="addressLine1" type="text" className="form-control mb-2" aria-label="Address Line 1" value={data.addressLine1 ?? ''} placeholder="Line 1..."
                                                onChange={(e) =>
                                                    setData({
                                                        ...data,
                                                        addressLine1: e.target.value,
                                                    })
                                                }
                                            />
                                            <input id="addressLine2" type="text" className="form-control mb-3" aria-label="Address Line 2" value={data.addressLine2 ?? ''} placeholder="Line 2..."
                                                onChange={(e) =>
                                                    setData({
                                                        ...data,
                                                        addressLine2: e.target.value,
                                                    })
                                                }
                                            />
                                            <div className="mb-3">
                                                <label htmlFor="city" className="form-label">City</label>
                                                <input id="city" type="text" className="form-control mt-2" aria-label="city" value={data.city ?? ''} placeholder="City..."
                                                    onChange={(e) =>
                                                        setData({
                                                            ...data,
                                                            city: e.target.value,
                                                        })
                                                    }
                                                />
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col">
                                                <div className="mb-3">
                                                    <label htmlFor="region" className="form-label">Region</label>
                                                    <input id="region" type="text" list="regionList" className="form-control mb-2" aria-label="Region" value={data.region ?? ''} placeholder="Type to search..."
                                                        onChange={(e) =>
                                                            setData({
                                                                ...data,
                                                                region: e.target.value,
                                                            })
                                                        }
                                                    />
                                                    <datalist id="regionList">
                                                        {getRegions().map(item => (
                                                            <option key={item.value} value={item.value}>{item.label}</option>
                                                        ))}
                                                    </datalist>
                                                </div>
                                            </div>
                                            <div className="col">
                                                <div className="mb-3">
                                                    <label htmlFor="postalCode" className="form-label">Postal Code</label>
                                                    <input id="postalCode" type="text" className="form-control mb-2" aria-label="Postal Code" value={data.postalCode ?? ''}
                                                        onChange={(e) =>
                                                            setData({
                                                                ...data,
                                                                postalCode: e.target.value,
                                                            })
                                                        }
                                                    />
                                                </div>
                                            </div>
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="country" className="form-label">Country</label>
                                            <select id="country" className="form-select" aria-label="Country" value={data.country ?? ''}
                                                onChange={(e) =>
                                                    setData({
                                                        ...data,
                                                        country: e.target.value,
                                                    })
                                                }
                                            >
                                                {getCountries().map(item => (
                                                    <option key={item.value} value={item.value}>{item.label}</option>
                                                ))}
                                            </select>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </form >
                <div className={id == 0 ? 'hidden' : 'row'}>
                    <div className="col text-end">
                        <button type="submit" className="btn btn-danger" onClick={() => deleteItem()}>
                            <FontAwesomeIcon icon="fa-solid fa-trash" className="me-2" /> Delete
                        </button>
                    </div>
                </div>
            </div >
        </>
    );
}

export default UserEdit;