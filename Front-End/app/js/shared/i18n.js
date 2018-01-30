function translateProvider($translateProvider) {
	$translateProvider.translations('en', {
		title: 'Farmworkers Safety Application',
		landing: {
			username_label: 'Phone Number',
			phone_hint: '(ex. 3109931234)',
			password_label: 'Password',
			btn_login: 'Login',
			btn_register: 'Register',
			btn_forgetpassword: 'Request new password',
			forget_password_label: 'Forgot your password?',
			back_to_login_label: 'Back to Login',
			username_required: 'Phone Number is required',
			password_required: 'Password is required'
		},
		register: {
			email_label: 'Email',
			email_required: 'Email is required.',
			phone_label: 'Phone',
			phone_hint: '(ex. 3109931234)',
			phone_required: 'Phone is required.',
			password_label: 'Password',
			password_required: 'Password is required.',
			confirm_password_label: 'Confirm Password',
			confirm_password_required: 'Confirm password is required.',
			confirm_password_mismatched: 'Confirm password is mismatched.',
			fname_label: 'First Name',
			lname_label: 'Last Name',
			btn_submit: 'Submit',
			offer_title: 'Farmworkers Safety Application offers you: ',
			offer_1: 'Temperature based notification',
			offer_2: 'Educational content to educate the farmers',
			offer_3: 'Easy to use',
			offer_4: 'The fourth one'
		},
		main_landing: {
			registered_farm_label: "Registered Farm",
			farm_address_label: "Farm's Address",
			farm_temperature_label: "Farm Temperature",
			phone_label: "Phone Number",
			email_label: "Email",
			birthdate_label: "Date of Birth",
			gender_label: "Gender",
			weight_label: "Weight",
			height_label: "Height",
			completed_edu_content_label: "Completed quizzes",
			completed_quiz_label: "Completed Educational content",
			welcome: "Welcome!",
			user_information: "User Information"
		},
		profile: {

			update_profile: {
				header: "Update Your Profile",
				phone_label: "Phone Number",
				email_label: "Email",
				birthdate_label: "Date of Birth",
				gender_label: "Gender",
				gender_required: "Gender is required.",
				weight_label: "Weight",
				height_label: "Height",
				email_label: 'Email',
				email_required: 'Email is required.',
				phone_label: 'Phone',
				phone_required: 'Phone is required.',
				fname_label: 'First Name',
				fname_required: 'First Name is required.',
				lname_label: 'Last Name',
				lname_required: 'Last Name is required.',
				farm_select_label: 'Choose Farm',
				farm_select_required: 'Farm must be chosen',
				noti_pref_label: 'Notification Preference',
				schedule_pref_label: 'Scheduling Preference',
				btn_update: 'Update Profile'
			},
			update_account: {
				header: "Update Your Account",
				current_password_label: 'Current Password',
				current_password_required: 'Current Password is required.',
				password_label: 'Password',
				password_required: 'Password is required.',
				confirm_password_label: 'Confirm Password',
				confirm_password_required: 'Confirm password is required.',
				confirm_password_mismatched: 'Confirm password is mismatched.',
				btn_update: 'Update Account',
			},
			update_farm: {
				header: "Update Your Farm",
				farm_select_label: 'Choose Farm',
				farm_select_required: 'Farm must be chosen',
				farm_name_label: "Register Farm",
				farm_address_label: "Farm's Address",
				btn_update: 'Update Farm',
			},
			update_preference: {
				header: "Update Your Notification Preferences",
				noti_type: 'Noitification Type',
				frequency: 'Frequency',
				language: "Language",
				btn_update: 'Update Preferences',
			}
		},
		admin: {
			landing: {
				farmworker_count_label: "Number of Farmworkers",
				farm_count_label: "Number of Farms",
				video_count_label: "Number of Videos",
				document_count_label: "Number of Documents",
				quiz_count_label: "Number of Quizs"
			},
			farmworker: {
				search_email_label: "Search by Email",
				search_phone_label: "Search by Phone Number",
				search_name_label: "Search by Name",
				btn_search: "Search",
				btn_back_to_result: "Back to Result",

			},
			farmowner: {
				search_email_label: "Search by Email",
				search_phone_label: "Search by Phone Number",
				search_name_label: "Search by Name",
				btn_search: "Search",
				btn_back_to_result: "Back to Result",

			},
			farm: {
                Farm_Information: "Farm's_Information",
                Create_A_New_Farm_label: "Create A New Farm ",
                Edit_The_Farm_label: "Edit The Farm",
                farm_name_label: "Farm Name",
                owner_label: "Farm Owner Name",
				owner_email_label: "Farm Owner Email",
				farm_address_label: "Farm Address",
				farm_city_label: "City",
                farm_State_label: "State",
                farm_Country_label: "Country",
				farm_zipcode_label: "Zip Code",
                farm_Save_Changes: "Save Changes",
                farm_Cancel: "Cancel",
                farm_Number_Of_FarmWorkers_In_That_Farm_label: "Number Of FarmWorkers In That Farm",
                btn_Edit: "Edit",
                btn_Delete: "Delete",
				farm_phone_label: "Phone Number",
				farm_emergency_phone_label: "Emergency Phone Number",
				farm_emergency_phone_hint: "(If the region of the farm has seperate Emergency)",
				btn_submit: "Submit"
                
			},
			education: {
				document: {
					upload_title: "Upload New Document",
					existing_title: "Existing Documents",
					btn_add: "Add",
					btn_edit: "Edit",
					btn_delete: "Delete",
					btn_disable: "Disable",
					btn_enable: "Enable",
					btn_update: "Update",
					btn_cancel_update: "Cancel"
				},
				video: {
					upload_title: "Upload New Video",
					existing_title: "Existing Videos",
					btn_add: "Add",
					btn_edit: "Edit",
					btn_delete: "Delete",
					btn_disable: "Disable",
					btn_enable: "Enable",
					btn_update: "Update",
					btn_cancel_update: "Cancel"
				},
				quiz: {
					upload_title: "Upload New Quiz",
					existing_title: "Existing Quizzes",
					btn_add: "Add",
					btn_create_quiz: "Create Quiz",
					btn_edit: "Edit",
					btn_delete: "Delete",
					btn_update: "Update",
					btn_cancel_update: "Cancel"
				}
			}
		},
		setting:{
			language:{
				label: "Language",
				english: "English",
				spanish: "Español"
			}
		},
		navigation:{
			login: "Login",
			logout: "Logout",
			home: "Home",
			educational: "Educational",
			profile: "Profile"
		},
		admin_navigation:{
			dashboard: "Dashboard",
			farms: "Farms",
			farmworkers: "Farm Workers",
			farmowners: "Farm Owners",
			educational_contents: "Educational Contents"
		},
		error:{
			"10001": "User has already been registered with this phone number",
			"10002": "Invalid Phone Number or Password",
			"10003": "User hasn't register the account using this phone number yet",
			"10004": "User hasn't register the account using this email yet",
			"10011": "You inputted wrong current password",
			"10999": "Unknown error occured, Please contact admin"
		}
	});
	$translateProvider.translations('es', {
		title: 'Farmworkers Safety Application ES',
		landing: {
			username_label: 'Username ES',
			phone_hint: '(ex. 3109931234 es)',
			password_label: 'Password ES',
			btn_login: 'Login ES',
			btn_register: 'Register ES',
			btn_forgetpassword: 'Request new password ES',
			forget_password_label: 'Forgot your password ES?',
			back_to_login_label: 'Back to Login ES',
			username_required: 'Phone Number is required ES',
			password_required: 'Password is required ES'
		},
		register: {
			email_label: 'Email ES',
			email_required: 'Email is required. ES',
			phone_label: 'Phone ES',
			phone_hint: '(ex. 3109931234 es)',
			phone_required: 'Phone is required. ES',
			password_label: 'Password ES',
			password_required: 'Password is required. ES',
			confirm_password_label: 'Confirm Password ES',
			confirm_password_required: 'Confirm password is required. ES',
			confirm_password_mismatched: 'Confirm password is mismatched. ES',
			fname_label: 'First Name ES',
			lname_label: 'Last Name ES',
			btn_submit: 'Submit ES',
			offer_title: 'Farmworkers Safety Application offers you:  ES',
			offer_1: 'Temperature based notification ES',
			offer_2: 'Educational content to educate the farmers ES',
			offer_3: 'Easy to use ES',
			offer_4: 'The fourth one ES'
		},
		main_landing: {
			registered_farm_label: "Registered Farm ES",
			farm_address_label: "Farm's Address ES",
			farm_temperature_label: "Farm Temperature ES",
			phone_label: "Phone Number ES",
			email_label: "Email ES",
			birthdate_label: "Date of Birth ES",
			gender_label: "Gender ES",
			weight_label: "Weight ES",
			height_label: "Height ES",
			completed_edu_content_label: "Completed quizzes ES",
			completed_quiz_label: "Completed Educational content ES",
			welcome: "Hola!",
			user_information: "User Information ES"
		},
		profile: {
			update_profile: {
				header: "Update Your Profile ES",
				phone_label: "Phone Number ES",
				email_label: "Email ES",
				birthdate_label: "Date of Birth ES",
				gender_label: "Gender ES",
				gender_required: "Gender is required. ES",
				weight_label: "Weight ES",
				height_label: "Height ES",
				email_label: 'Email ES',
				email_required: 'Email is required. ES',
				phone_label: 'Phone ES',
				phone_required: 'Phone is required. ES',
				fname_label: 'First Name ES',
				fname_required: 'First Name is required. ES',
				lname_label: 'Last Name ES',
				lname_required: 'Last Name is required. ES',
				farm_select_label: 'Choose Farm ES',
				farm_select_required: 'Farm must be chosen ES',
				noti_pref_label: 'Notification Preference ES',
				schedule_pref_label: 'Scheduling Preference ES',
				btn_update: 'Update Profile ES',
			},
			update_account: {
				header: "Update Your Account ES",
				current_password_label: 'Current Password ES',
				current_password_required: 'Current Password is required. ES',
				password_label: 'Password ES',
				password_required: 'Password is required. ES',
				confirm_password_label: 'Confirm Password ES',
				confirm_password_required: 'Confirm password is required. ES',
				confirm_password_mismatched: 'Confirm password is mismatched. ES',
				btn_update: 'Update Account ES',
			},
			update_farm: {
				header: "Update Your Farm ES",
				farm_select_label: 'Choose Farm ES',
				farm_select_required: 'Farm must be chosen ES',
				farm_name_label: "Register Farm ES",
				farm_address_label: "Farm's Address ES",
				btn_update: 'Update Farm ES',
			},
			update_preference: {
				header: "Update Your Preferences ES",
				noti_type: 'Noitification Type ES',
				frequency: 'Frequency ES',
				language: "Language ES",
				header: "Update Your Notification Preferences ES",
				btn_update: 'Update Preferences ES',
			}
		},
		admin: {
			landing: {
				farmworker_count_label: "Number of Farmworkers ES",
				farm_count_label: "Number of Farms ES",
				video_count_label: "Number of Videos ES",
				document_count_label: "Number of Documents ES",
				quiz_count_label: "Number of Quizs ES"
			},
			farmworker: {
				search_email_label: "Search by Email ES",
				search_phone_label: "Search by Phone Number ES",
				search_name_label: "Search by Name ES",
				btn_search: "Search ES",
			},
			farm: {
				Farm_Information: "Farm's_Information ES",
                Create_A_New_Farm_label: "Create A New Farm ES",
                Edit_The_Farm_label: "Edit The Farm ES",
                farm_name_label: "Farm Name ES",
                owner_label: "Farm Owner Name ES",
				owner_email_label: "Farm Owner Email ES",
				farm_address_label: "Farm Address ES",
				farm_city_label: "City ES",
                farm_State_label: "State ES",
                farm_Country_label: "Country ES",
				farm_zipcode_label: "Zip Code ES",
                farm_Save_Changes: "Save Changes ES",
                farm_Cancel: "Cancel ES",
                farm_Number_Of_FarmWorkers_In_That_Farm_label: "Number Of FarmWorkers In That Farm ES",
                btn_Edit: "Edit ES",
                btn_Delete: "Delete ES",
				farm_phone_label: "Phone Number ES",
				farm_emergency_phone_label: "Emergency Phone Number ES",
				farm_emergency_phone_hint: "(If the region of the farm has seperate Emergency) ES",
				btn_submit: "Submit ES"
                
			},
			education: {
				document: {
					upload_title: "Upload New Document ES",
					existing_title: "Existing Documents ES",
					btn_add: "Add ES",
					btn_edit: "Edit ES",
					btn_delete: "Delete ES",

				},
				video: {
					upload_title: "Upload New Video ES",
					existing_title: "Existing Videos ES",
					btn_add: "Add ES",
					btn_edit: "Edit ES",
					btn_delete: "Delete ES"

				},
				quiz: {
					upload_title: "Upload New Quiz ES",
					existing_title: "Existing Quizzes ES",
					btn_add: "Add ES",
					btn_create_quiz: "Create Quiz ES",
					btn_edit: "Edit ES",
					btn_delete: "Delete ES",

				}
			}
		},
		setting:{
			language:{
				label: "Idioma",
				english: "English",
				spanish: "Español"
			}
		},
		navigation:{
			login: "Login",
			logout: "Logout",
			home: "Home",
			educational: "Educational",
			profile: "Profile"
		},
		admin_navigation:{
			dashboard: "Dashboard ES",
			farms: "Farms ES",
			farmworkers: "Farm Workers ES",
			farmowners: "Farm Owners ES",
			educational_contents: "Educational Contents ES"
		},
		error:{
			"10001":"User has already been registered with this phone number ES",
			"10002":"Invalid Username or Password ES",
			"10003":"User hasn't register the account using this phone number yet ES",
			"10004":"User hasn't register the account using this email yet ES",
			"10011": "You inputted wrong current password ES",
			"10999":"Unknown error occured, Please contact admin ES"
		}
	});
	$translateProvider.preferredLanguage('en');
}
angular.module("angularApp").config(translateProvider);