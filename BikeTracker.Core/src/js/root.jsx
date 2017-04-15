import React from 'react';
import { MainNav } from './navbar.jsx';
import { Home } from './home/home.jsx';
import { BrowserRouter as Router, Route } from 'react-router-dom';

export class Root extends React.Component {
    render() {
        return (
            <Router>
                <div>
                    <MainNav />
                    <Route path='/' component={Home} />
                </div>
            </Router>);

    }
}
