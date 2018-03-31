import { Store } from "MyRE/Models/Store";
import * as React from "react";
import { Option } from "ts-option";
import { Row, Alert } from "reactstrap";

interface IProps {
    message: Option<Store.AlertMessage>;
    preformatted?: boolean;
}

export class AlertRow extends React.Component<IProps> {
    public render() {
        if (this.props.message.isDefined) {
            return (
                <Row>
                    <Alert color={this.props.message.get.level}>
                        {this.props.preformatted && <pre>{this.props.message.get.message}</pre>}
                        {(!this.props.preformatted) && this.props.message.get.message}

                    </Alert>
                </Row>
            );
        } else {
            return null;
        }   
    }
}