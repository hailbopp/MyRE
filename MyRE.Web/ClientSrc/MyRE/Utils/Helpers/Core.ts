import _deepEqual = require('deep-equal');

export function deepEqual(o1: any, o2: any) {
    return _deepEqual(o1, o2, { strict: true });
}

export function deepCopy(o: any) {
    return JSON.parse(JSON.stringify(o));
}