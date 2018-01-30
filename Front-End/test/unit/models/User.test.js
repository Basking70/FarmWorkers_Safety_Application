describe("addFarmworkerComponent test", function () {
    var User;
    beforeEach(module('angularApp'));

    beforeEach(inject(function (_User_) {
        User = _User_;
    }));

    function stubUserJson() {
        return {
            "QuizAttemptsUsers": [

            ],
            "UserCommunicationPreferences": [

            ],
            "UserEducationalContents": [

            ],
            "UserEmergencyContacts": [

            ],
            "UserFarms": [
                {
                    "Farm": {
                        "UserFarms": [

                        ],
                        "WeatherHistories": [

                        ],
                        "FarmID": 10,
                        "FarmName": "FarmTEst",
                        "FarmHouseNumberStreetAddress": "f2",
                        "FarmCity": "22123",
                        "FarmState": "f",
                        "FarmCountry": "f",
                        "FarmZipCode": 90007,
                        "FarmLatitute": "",
                        "FarmLongitude": "",
                        "FarmTemperatureMin": "",
                        "FarmTemperatureMax": "",
                        "IsActive": "0"
                    },
                    "UserInFarmID": 60,
                    "UserID": 11,
                    "FarmID": 10,
                    "UpdateDate": null,
                    "IsLatest": 1,
                    "User": null
                }
            ],
            "UserID": 11,
            "UserType": "Farm Worker",
            "UserName": "Theerapat",
            "UserLastName": "Chawannakul",
            "UserDateOfBirth": null,
            "UserHeight": "1",
            "UserWeight": "1",
            "UserGender": null,
            "UserEmail": null,
            "UserPhoneNumber": "123456",
            "UserWorkLocation": null,
            "UserMemberSince": null,
            "IsActive": null,
            "CurrentFarm": {
                "UserFarms": [
                    {
                        "UserInFarmID": 60,
                        "UserID": 11,
                        "FarmID": 10,
                        "UpdateDate": null,
                        "IsLatest": 1,
                        "User": null
                    }
                ],
                "WeatherHistories": [

                ],
                "FarmID": 10,
                "FarmName": "FarmTEst",
                "FarmHouseNumberStreetAddress": "f2",
                "FarmCity": "22123",
                "FarmState": "f",
                "FarmCountry": "f",
                "FarmZipCode": 90007,
                "FarmLatitute": "",
                "FarmLongitude": "",
                "FarmTemperatureMin": "",
                "FarmTemperatureMax": "",
                "IsActive": "0"
            }


        }
    }
    it("convert json to user should convert correctly", function () {
        var user = new User(stubUserJson());
        expect(user.UserName).to.equal('Theerapat');
        expect(user.UserFarms).to.have.lengthOf(1);
    });


});