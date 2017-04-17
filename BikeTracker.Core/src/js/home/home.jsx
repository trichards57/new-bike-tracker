import React from 'react';

import { Jumbotron, Button, Card, CardBlock, CardTitle, CardText, CardDeck } from 'reactstrap';
import { Link } from 'react-router-dom';

export class Home extends React.Component {
    render() {
        return (
            <div className="container">
                <Jumbotron>
                    <h1 className="display-3">SJA Tracker</h1>
                    <p className="lead">Welcome to the St Ambulance SWR Central District Tracker</p>
                    <Link to="/app/login" className="btn btn-lg btn-sja-dark-green">Log in »</Link>
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
            </div>
        );
    }
}
