import * as React from 'react';

import { Jumbotron, Button, Card, CardBlock, CardTitle, CardText, CardDeck, Container } from 'reactstrap';
import { Link, RouteComponentProps } from 'react-router-dom';
import { AppStore } from "../store/app-store";
import { inject, observer } from "mobx-react";

interface IHomeProps extends RouteComponentProps<any> {
    appStore : AppStore
}

@inject("appStore") @observer
export class Home extends React.Component<IHomeProps, undefined> {
    render() {
        let jumpoButton = this.props.appStore.user.authenticated ?
            <Link to="/app/map" className="btn btn-lg btn-sja-dark-green">Go to Map »</Link> :
            <Link to="/app/login" className="btn btn-lg btn-sja-dark-green">Log in »</Link>;


        return (
            <Container>
                <Jumbotron>
                    <h1 className="display-3">SJA Tracker</h1>
                    <p className="lead">Welcome to the St Ambulance SWR Central District Tracker</p>
                    {jumpoButton}
                </Jumbotron>

                <CardDeck className="front-page">
                    <Card>
                        <CardBlock>
                            <CardTitle>New Users</CardTitle>
                            <CardText>
                                If you have never used this site before, you will need to be issued with a username and password.
                                </CardText>
                            <Link to="/Contact" className="btn btn-secondary btn-block">Request Account »</Link>
                        </CardBlock>
                    </Card>
                    <Card>
                        <CardBlock>
                            <CardTitle>Get the Trackers</CardTitle>
                            <CardText>
                                There are a set of apps to enable your phone to be tracked from this website.
                                </CardText>
                            <Link to="" className="btn btn-secondary btn-block disabled" disabled>Coming Soon</Link>
                        </CardBlock>
                    </Card>
                    <Card>
                        <CardBlock>
                            <CardTitle>Privacy Policy</CardTitle>
                            <CardText>
                                To get a better idea of how this website is used and how it can be made better, we collect some information about what you do with this site.
                                </CardText>
                            <Link to="/Policies" className="btn btn-secondary btn-block">Find out More »</Link>
                        </CardBlock>
                    </Card>
                </CardDeck>
            </Container>
        );
    }
}
