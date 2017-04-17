import React from 'react';
import { Navbar, NavbarToggler, NavbarBrand, Collapse, Nav, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';

export class MainNav extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            isOpen: false
        };
    }

    toggle() {
        this.setState((prev, props) => ({
            isOpen: !prev.isOpen
        }));
    }

    render() {
        return (
            <Navbar color="sja-dark-green" inverse toggleable>
                <NavbarToggler right onClick={() => this.toggle()} />
                <NavbarBrand href="/">SJA Tracker</NavbarBrand>
                <Collapse isOpen={this.state.isOpen} navbar>
                    <Nav navbar>
                        <NavItem>
                            <Link className="nav-link" to="/">Home</Link>
                        </NavItem>
                        <NavItem>
                            <Link className="nav-link" to="/Map">Map</Link>
                        </NavItem>
                        <NavItem>
                            <Link className="nav-link" to="/Contact">Contact</Link>
                        </NavItem>
                    </Nav>
                    <Nav navbar className="ml-auto">
                        <NavItem>
                            <Link className="nav-link" to="/app/login">Log in</Link>
                        </NavItem>
                    </Nav>
                </Collapse>
            </Navbar>);
    }
}
