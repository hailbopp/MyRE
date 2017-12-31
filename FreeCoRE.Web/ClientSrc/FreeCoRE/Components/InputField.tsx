import * as React from 'react';
import { FormGroup, Label, Input, InputGroup, FormGroupProps, Col, ColProps } from 'reactstrap';

export type InputType =
    | 'text'
    | 'email'
    | 'select'
    | 'file'
    | 'radio'
    | 'checkbox'
    | 'textarea'
    | 'button'
    | 'reset'
    | 'submit'
    | 'date'
    | 'datetime-local'
    | 'hidden'
    | 'image'
    | 'month'
    | 'number'
    | 'range'
    | 'search'
    | 'tel'
    | 'url'
    | 'week'
    | 'password'
    | 'datetime'
    | 'time'
    | 'color';

interface InputFieldProps {
    onChange: React.ChangeEventHandler<HTMLInputElement>;
    value: string;
    label: string;
    name: string;
    type: InputType;
    placeholder?: string;
    row?: boolean;

    xs?: string;
    sm?: string;
    md?: string;
}

export class InputField extends React.Component<InputFieldProps> {
    public render() {


        return (
            <FormGroup row={this.props.row}>
                <Label for={this.props.name}>{this.props.label}</Label>

                <Col xs={this.props.xs} sm={this.props.sm} md={this.props.md}>
                    <Input type={this.props.type}
                        name={this.props.name}
                        onChange={this.props.onChange}
                        value={this.props.value}
                        placeholder={this.props.placeholder} />
                </Col>
            </FormGroup>
        );
    }
}

