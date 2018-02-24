import { ProjectSource, DeviceInfo, ExpressionTree } from "MyRE/Api/Models";
import { List } from "immutable";

import * as ParserTypes from 'MyRE/Utils/Models/Parser';
import { Program } from "MyRE/Utils/Models/DslModels";
var parser = require('MyRE/Utils/MyreLisp.pegjs') as ParserTypes.Parser;

export const parseSource = (source: string): ExpressionTree => {
    try {
        return parser.parse(source, {}) as ExpressionTree;
    } catch (e) {
        console.warn(e);
        return [];
    }
}

const refString = (baseString: string) => `\`${baseString}\``;

export const convertDisplaySourceToInternalFormat: (baseSource: ProjectSource, source: string, devices: DeviceInfo[]) => ProjectSource = (baseSource, source, devices) => {
    const newSource: ProjectSource = Object.assign({}, baseSource);

    for (var d of devices) {
        newSource.Source = newSource.Source.replace(refString(d.DisplayName), refString(d.DeviceId));
    }

    newSource.ExpressionTree = parseSource(newSource.Source);

    return newSource;
}

export const convertInternalSourceToDisplayFormat: (source: ProjectSource, devices: DeviceInfo[]) => ProjectSource = (source, devices) => {
    const newSource: ProjectSource = Object.assign({}, source);

    for (var d of devices) {
        newSource.Source = newSource.Source.replace(refString(d.DeviceId), refString(d.DisplayName));
    }
    
    newSource.ExpressionTree = parseSource(newSource.Source);

    return newSource;
}

//type SyncProjectSourcesResult = { display: ProjectSource; internal: ProjectSource; };
//export const syncProjectSources = (display: ProjectSource, internal: ProjectSource, devices: List<DeviceInfo>): SyncProjectSourcesResult => ({
//    display: convertInternalSourceToDisplayFormat(internal, devices),
//    internal: convertDisplaySourceToInternalFormat(display, devices)
//});