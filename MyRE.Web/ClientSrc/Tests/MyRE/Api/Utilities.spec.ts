/*
import { assert } from 'chai';

import * as Utilities from "MyRE/Api/Utilities";
import { ApiResult } from 'MyRE/Api/Models/Results';
import { User } from 'MyRE/Api/Models';
import { List } from 'immutable';

describe("Api", () => {
    describe("Utilities", () => {
        describe("#convertArrayToImmutableList()", () => {
            const getTestResult = () => {
                const res: ApiResult<Array<User>> = {
                    result: 'success',
                    data: [
                        {
                            Email: 'a@a.com',
                            UserId: 'E98D500A-8861-4EA9-913B-DDFB6215FC01',
                        },
                        {
                            Email: 'b@b.com',
                            UserId: 'E98D500A-8861-4EA9-913B-DDFB6215FC02',
                        }
                    ]
                };

                return res;
            }

            it("should return an ApiResult<List>", () => {
                const original = getTestResult();  

                const result = Utilities.convertArrayToImmutableList(original);
                
                assert.isTrue(result.result === "success" && List.isList(result.data));
            });
        });
    });
});
*/