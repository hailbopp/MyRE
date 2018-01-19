import * as React from 'react';
import { connect, Dispatch } from 'react-redux';
import { Store } from 'MyRE/Models/Store';

interface IOwnProps {}

export type IPageHeaderProperties = IOwnProps;

class PageHeaderComponent extends React.PureComponent<IPageHeaderProperties> {
	public render() {
		return (
            <h3>{this.props.children}</h3>
		);
	}
}

export const PageHeader =(
        PageHeaderComponent);
