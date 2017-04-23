import * as React from 'react';
import { Card, CardBlock, CardTitle, Button, Form, FormGroup, Label, Input, InputGroup, InputGroupAddon, } from 'reactstrap';
import { Link } from 'react-router-dom';

interface ILoginBoxState {
    username: string;
    password: string;
}

interface ILoginBoxProps {
    onLogin: (username: string, password: string) => void;
}

export class LoginBox extends React.Component<ILoginBoxProps, ILoginBoxState> {
    constructor(props: ILoginBoxProps) {
        super(props);
        this.state = {
            username: "",
            password: ""
        };
    }

    private updateUsername(event: React.ChangeEvent<HTMLInputElement>) {
        this.setState({ username: event.target.value });
    }

    private updatePassword(event: React.ChangeEvent<HTMLInputElement>) {
        this.setState({ password: event.target.value });
    }

    private submitForm(event: React.SyntheticEvent<HTMLFormElement>) {
        event.preventDefault();

        this.props.onLogin(this.state.username, this.state.password);
    }

    public render() {
        return (
            <div className="container">
                <div className="d-flex justify-content-center">
                    <Card className="login-card">
                        <CardBlock>
                            <CardTitle>Log In</CardTitle>
                            <Form>
                                <FormGroup>
                                    <InputGroup>
                                        <InputGroupAddon><span className="fa fa-fw fa-at"></span></InputGroupAddon>
                                        <Input type="email" name="Email" id="email" placeholder="Email" value={this.state.username} onChange={(e) => this.updateUsername(e)} />
                                    </InputGroup>
                                </FormGroup>
                                <FormGroup>
                                    <InputGroup>
                                        <InputGroupAddon><span className="fa fa-fw fa-lock"></span></InputGroupAddon>
                                        <Input type="password" name="Password" id="password" placeholder="Password" value={this.state.password} onChange={(e) => this.updatePassword(e)} />
                                    </InputGroup>
                                </FormGroup>
                                <Button type="submit" className="btn-block" onClick={(e) => this.submitForm(e)}>Log in</Button>
                                <Link to="/app/reset-password">Forgotten your password?</Link>
                            </Form>
                        </CardBlock>
                    </Card>
                </div>
            </div>
        );
    }
}
