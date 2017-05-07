import * as React from "react";
import { Progress, Row, Col, Card, CardBlock } from "reactstrap";

interface ILoadingDisplay {
}

export class LoadingDisplay extends React.Component<ILoadingDisplay, undefined> {
    public render() {
        return (
            <Row className="waiting-display">
                <Col md={{ size: 4, offset: 4 }}>
                    <Card>
                        <CardBlock>
                            <div className="text-center">
                                One moment please...
                                <Progress striped animated value={100}></Progress>
                            </div>
                        </CardBlock>
                    </Card>
                </Col>
            </Row>
        );
    }
}