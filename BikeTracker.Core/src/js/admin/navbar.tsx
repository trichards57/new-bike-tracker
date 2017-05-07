import * as React from 'react';
import { Navbar, NavbarToggler, NavbarBrand, Collapse, Nav, NavItem, NavLink } from 'reactstrap';
import { Form, FormGroup, InputGroup, InputGroupAddon, Input } from 'reactstrap';
import { Link } from 'react-router-dom';

interface IAdminNavProps {
}

interface IAdminNavState {
    isOpen: boolean;
}

export class AdminNav extends React.Component<IAdminNavProps, IAdminNavState> {
    constructor(props: IAdminNavProps) {
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
                <Navbar color="faded" light toggleable>
                    <NavbarToggler right onClick={() => this.toggle()} />
                    <NavbarBrand href="/app/admin">
                        Manager Users
                </NavbarBrand>

                    <Collapse isOpen={this.state.isOpen} navbar>
                        <Nav className="mr-auto" navbar>
                            <NavItem>
                                <Link className="nav-link" to="/app/admin/newUser">New User</Link>
                            </NavItem>
                        </Nav>
                        <Form inline>
                            <FormGroup>
                                <InputGroup>
                                    <InputGroupAddon>
                                        <span className="fa fa-fw fa-search"></span>
                                    </InputGroupAddon>
                                    <Input type="text" placeholder="Search" />
                                </InputGroup>
                            </FormGroup>
                        </Form>
                    </Collapse>
                </Navbar>
            );
    }
}
