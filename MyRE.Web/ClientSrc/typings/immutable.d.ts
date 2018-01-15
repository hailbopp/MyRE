import { Collection } from "immutable";

declare module 'immutable' {
    export interface Iterable<K, V> {
        map<M>(
            mapper: (value: V, key: K, iter: /*this*/Iterable<K, V>) => M,
            context?: any
        ): /*this*/Iterable<K, M>
    }
}