import { ProjectSource, DeviceInfo, ExpressionTree } from "MyRE/Api/Models";
import { List } from "immutable";

import * as ParserTypes from 'MyRE/Utils/Models/Parser';
import { Program } from "MyRE/Utils/Models/DslModels";
var parser = require('MyRE/Utils/MyreLisp.pegjs') as ParserTypes.Parser;

export const parseSource = (source: string): ExpressionTree => {
    try {
        return parser.parse(source, {}) as ExpressionTree;
    } catch (e) {
        console.error(e);
        return [];
    }
}

export const convertDisplaySourceToInternalFormat: (source: ProjectSource, devices: List<DeviceInfo>) => ProjectSource = (source, devices) => {
    let newSource: ProjectSource = Object.assign({}, source);

    devices.forEach(d => {
        if (d) {
            newSource.Source = newSource.Source.replace(d.DisplayName, d.DeviceId);
        }
    });
    newSource.ExpressionTree = parseSource(newSource.Source);

    return newSource;
}

export const convertInternalSourceToDisplayFormat: (source: ProjectSource, devices: List<DeviceInfo>) => ProjectSource = (source, devices) => {
    let newSource: ProjectSource = Object.assign({}, source);

    devices.forEach(d => {
        if (d) {
            newSource.Source = newSource.Source.replace(d.DeviceId, d.DisplayName);
        }
    });
    newSource.ExpressionTree = parseSource(newSource.Source);

    return newSource;
}

type SyncProjectSourcesResult = { display: ProjectSource; internal: ProjectSource; };
export const syncProjectSources = (display: ProjectSource, internal: ProjectSource, devices: List<DeviceInfo>): SyncProjectSourcesResult => ({
    display: convertInternalSourceToDisplayFormat(internal, devices),
    internal: convertDisplaySourceToInternalFormat(display, devices)
});