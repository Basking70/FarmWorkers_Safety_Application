

		SELECT U.UserID, U.UserName, U.UserLastName, U.UserType,
			   U.UserDateOfBirth, U.UserHeight, U.UserWeight,
			   U.UserGender, U.UserEmail, U.UserPhoneNumber, U.UserWorkLocation,
			   U.UserMemberSince, U.IsActive, 
			   (SELECT F0.FarmID
				FROM Farm as F0, dbo.[User] as U0, UserFarm as UF0 
				WHERE F0.IsActive = '1' AND
					  F0.FarmID = UF0.FarmID AND
					  UF0.UserID = U0.UserID AND
					  UF0.IsLatest = '1') as FarmID 
		FROM  dbo.[User] as U 
		WHERE U.IsActive = '1' AND U.UserPhoneNumber = '13054501490'