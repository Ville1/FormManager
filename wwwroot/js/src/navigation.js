import { createRoot } from 'react-dom/client';
import React from 'react';

class Navigation extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <nav className="navbar navbar-expand-lg navbar-main">
                <div className="container-fluid">
                    <button
                        className="navbar-toggler"
                        type="button"
                        data-bs-toggle="collapse"
                        data-bs-target="#navbarSupportedContent"
                        aria-controls="navbarSupportedContent"
                        aria-expanded="false"
                        aria-label="Toggle navigation"
                    >
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className="collapse navbar-collapse" id="navbarSupportedContent">
                        <ul className="navbar-nav me-auto mb-2 mb-lg-0">
                            <li className="nav-item">
                                <a className="nav-link active" aria-current="page" href={baseUrl + '/Main'}>{localization.FrontPageTitle}</a>
                            </li>
                            <li className="nav-item dropdown">
                                <a className="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                                    {localization.Forms}
                                </a>
                                <ul className="dropdown-menu">
                                    <li>
                                        <h6 class="dropdown-header">{localization.VideoGames}</h6>
                                    </li>
                                    <li>
                                        <a className="dropdown-item" href="#">{localization.New}</a>
                                    </li>
                                    <li>
                                        <a className="dropdown-item" href={baseUrl + '/VideoGame/Index'}>{localization.Browse}</a>
                                    </li>
                                </ul>
                            </li>
                        </ul>
                        <ul className="navbar-nav ms-auto mb-2 mb-lg-0">
                            <li className="nav-item">
                                <a className="nav-link active" aria-current="page" href={baseUrl + '/Logout'} >{localization.Logout}</a>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>
        );
    }
}

createRoot(document.getElementById('navigation')).render(<Navigation />);