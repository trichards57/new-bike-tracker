import React from 'react';
import { Card, CardBlock, CardTitle, Button, Form, FormGroup, Label, Input, InputGroup, InputGroupAddon } from 'reactstrap';
import { Link } from 'react-router-dom';
import Cookies from 'js-cookie';

export class Login extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            xsrfToken: Cookies.get('XSRF-TOKEN')
        };
    }

    render() {
        return (
            <div className="container">
                <div className="d-flex justify-content-center">
                    <Card id="login-card">
                        <CardBlock>
                            <CardTitle>Log In</CardTitle>
                            <Form action="/Account/Login" method="post" >
                                <Input type="hidden" name="__RequestVerificationToken" value={this.state.xsrfToken} />
                                <FormGroup>
                                    <InputGroup>
                                        <InputGroupAddon><span className="fa fa-fw fa-at"></span></InputGroupAddon>
                                        <Input type="email" name="Email" id="email" placeholder="Email" />
                                    </InputGroup>
                                </FormGroup>
                                <FormGroup>
                                    <InputGroup>
                                        <InputGroupAddon><span className="fa fa-fw fa-lock"></span></InputGroupAddon>
                                        <Input type="password" name="Password" id="password" placeholder="Password" />
                                    </InputGroup>
                                </FormGroup>
                                <FormGroup check>
                                    <Label check>
                                        <Input type="checkbox" name="RememberMe" />{' '}
                                        Remember me
                                    </Label>
                                </FormGroup>
                                <Button type="submit" className="btn-block">Log in</Button>
                                <Link to="/app/reset-password">Forgotten your password?</Link>
                            </Form>
                        </CardBlock>
                    </Card>
                </div>
            </div>
        );
    }
}
