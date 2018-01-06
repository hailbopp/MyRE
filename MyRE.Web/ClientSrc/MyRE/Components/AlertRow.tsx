import { Store } from "MyRE/Models/Store";
import * as React from "react";
import { Option } from "ts-option";
import { Row, Alert } from "reactstrap";

interface IProps {
    message: Option<Store.AlertMessage>;
}

export class AlertRow extends React.Component<IProps> {
    public render() {
        if (this.props.message.isDefined) {
            return (
                <Row>
                    <Alert color={this.props.message.get.level}>
                        {this.props.message.get.message}
                    </Alert>
                </Row>
            );
        } else {
            return null;
        }   
    }
}