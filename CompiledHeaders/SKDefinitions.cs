using System;

namespace SKMap {

	public enum SKConnectivityMode {
		Online,
		Offline
	}

	public enum SKPositionsLoggingType {
		LOG,
		GPX
	}

	public enum SKGPSAccuracyLevel {
		Unknown = 0,
		Bad = 1,
		Medium = 2,
		Good = 3
	}

	public enum SKMetaDataDownloadStatus {
		DownloadError = -1,
		NotExist = 0,
		Downloaded = 1,
		DownloadInProgress = 2
	}

	public enum SKAddPackageResult {
		Success = 0,
		MissingTxgFile = 1,
		MissingSkmFile = 1 << 1,
		MissingNgiFile = 1 << 2,
		MissingNgiDatFile = 1 << 3,
		CannotEraseFile = 1 << 4
	}

	public enum SKPosition {
		AddPackageResultSuccess = 0,
		AddPackageResultMissingTxgFile = 1,
		AddPackageResultMissingSkmFile = 1 << 1,
		AddPackageResultMissingNgiFile = 1 << 2,
		AddPackageResultMissingNgiDatFile = 1 << 3,
		AddPackageResultCannotEraseFile = 1 << 4
	}

	public enum SKMapDisplayMode {
		2D,
		3D
	}

	public enum SKMapFollowerMode {
		None,
		Position,
		PositionPlusHeading,
		Navigation
	}

	public enum SKOrientationIndicatorType {
		None,
		Default,
		CustomImage
	}

	public enum SKMapDetailLevel {
		Full = 0,
		Light = 1
	}

	public enum SKCoordinateRegion  {
		MapDetailLevelFull = 0,
		MapDetailLevelLight = 1
	}

	public enum SKAnnotationType {
		Purple = 32,
		Blue = 33,
		Green = 38,
		Red = 39,
		DestinationFlag = 47,
		Marker = 64
	}

	public enum SKAnimationType {
		None = 0,
		PinDrop = 1,
		PopOut = 2,
		PulseAnimation = 3
	}

	public enum SKAnimationEasingType {
		eLinear,
		eInQuad,
		eOutQuad,
		eInOutQuad,
		eInCubic,
		eOutCubic,
		eInOutCubic,
		eInQuart,
		eOutQuart,
		eInOutQuart,
		eInQuint,
		eOutQuint,
		eInOutQuint,
		eInSine,
		eOutSine,
		eInOutSine,
		eInExpo,
		eOutExpo,
		nEaseInOutExpo
	}

	public enum SKPOIDisplayingOption {
		None = 0,
		City = 1 << 0,
		General = 1 << 1,
		Important = 1 << 2
	}

	public enum SKRouteID : uint {
		POIDisplayingOptionNone = 0,
		POIDisplayingOptionCity = 1 << 0,
		POIDisplayingOptionGeneral = 1 << 1,
		POIDisplayingOptionImportant = 1 << 2
	}

	public enum SKRouteMode {
		CarShortest = 0,
		CarFastest = 1,
		CarEfficient = 2,
		Pedestrian = 3,
		BicycleFastest = 4,
		BicycleShortest = 5,
		BicycleQuietest = 6
	}

	public enum SKStreetType {
		Undefined = 0,
		Bridleway = 1,
		Construction = 2,
		Crossing = 3,
		Cycleway = 4,
		Ferry = 5,
		Footway = 6,
		Ford = 7,
		Living_street = 8,
		Motorway = 9,
		Motorway_link = 10,
		Path = 11,
		Pedestrian = 12,
		Primary = 13,
		Primary_link = 14,
		Residential = 15,
		Road = 16,
		Secondary = 17,
		Secondary_link = 18,
		Service = 19,
		Steps = 20,
		Tertiary = 21,
		Tertiary_link = 22,
		Track = 23,
		Trunk = 24,
		Trunk_link = 25,
		Unclassified = 26,
		FerryPed = 310,
		Residential_limited = 311,
		UnpavedTrack = 377,
		Permissive = 378,
		Destination = 379,
		Pier = 380
	}

	public enum SKTransportMode {
		Pedestrian = 0,
		Bicycle = 1,
		Car = 2
	}

	public enum SKRouteConnectionMode {
		Online = 0,
		Offline = 1,
		Hybrid = 2
	}

	public enum SKNavigationType {
		Real,
		Simulation,
		SimulationFromLogFile
	}

	public enum SKRoutingErrorCode {
		NoResultsFound = 601,
		MissingArgument = 610,
		InvalidArgument = 611,
		UnsupportedServerCommand = 621,
		UnsupportedRequestType = 631,
		SameStartAndDestinationCoordinate = 680,
		InvalidStartCoordinate = 681,
		InvalidDestinationCoordinate = 682,
		CannotBeCalculated = 683,
		InvalidViaPoint = 684,
		InternalError = 690,
		ExtComputationCanceled = 901,
		RouteCalculationTurnedOffline = 909,
		InternetTurnedOff = 910
	}

	public enum GPSDataFileType {
		SKGPXFileType,
		SKKMLFileType
	}

	public enum SKGPXComponentType {
		Route,
		Track,
		Waypoint
	}

	public enum SKDistanceFormat {
		Metric = 0,
		MilesFeet,
		MilesYards
	}

	public enum SKMapLanguage {
		LOCAL = 0,
		EN = 1,
		DE = 2,
		FR = 3,
		IT = 4,
		ES = 5,
		RU = 6,
		TR = 7
	}

	public enum SKRealReachUnit {
		Second = 0,
		Meter = 1,
		MiliAmp = 2
	}

	public enum SKMapInternationalizationOption {
		None = 0,
		Local = 1,
		Transliterated = 2,
		International = 3
	}

	public enum SKSearchMode {
		Online = 0,
		Offline = 1,
		Hybrid = 2
	}

	public enum SKListLevel {
		CountryList = 0,
		StateList = 1,
		CityList = 2,
		StreetList = 3,
		HouseNumberList = 4,
		InvalidListLevel = 5
	}

	public enum SKSearchResultType {
		Country = 0,
		State = 1,
		City = 3,
		ZipCode = 4,
		Suburb = 5,
		Neighbourhood = 6,
		Hamlet = 7,
		Street = 8,
		POI = 9,
		HouseNumber = 10,
		WikiPoi,
		CountryCode,
		StateCode
	}

	public enum SKSearchResultSortType {
		MatchSort,
		ProximitySort
	}

	public enum SKMapSearchStatus : uint32_t {
		NoError = 0,
		NoSearchComponent,
		NoMapInformation,
		issingSearchFile,
		issingSearchTerm,
		issingLanguageCode,
		UnknownError
	}

	public enum SKSearchType {
		POI = 0,
		treet = 1,
		All = 2
	}

	public enum SKPOIType {
		CategorySearch = 0,
		LocalSearch = 1
	}

	public enum SKPOICategory {
		Airport = 32,
		Aerodrome = 348,
		FerryTerminal = 97,
		Camping = 370,
		Helipad = 350,
		ThemePark = 208,
		AmusementPark = 208,
		WaterPark = 224,
		CampSite = 57,
		Campground = 57,
		Station = 194,
		TrainStation = 194,
		Stadium = 325,
		Hospital = 124,
		Attraction = 39,
		Garden = 109,
		GraveYard = 114,
		Cemetery = 114,
		Information = 128,
		Marina = 137,
		NatureReserve = 146,
		Park = 157,
		Prison = 175,
		Shelter = 185,
		Stadium2 = 192,
		Townhall = 213,
		CityHall = 213,
		LocalGovernmentOffice = 213,
		Zoo = 232,
		Aquarium = 232,
		EvCharging = 360,
		BusStation = 54,
		CarRental = 60,
		CarSharing = 62,
		CarRental2 = 62,
		Cinema = 68,
		MovieTheater = 68,
		College = 75,
		ConcertHall = 77,
		Embassy = 94,
		Food = 103,
		Fountain = 104,
		Fuel = 105,
		Hotel = 126,
		Library = 135,
		Mall = 136,
		ShoppingMall = 136,
		Museum = 144,
		Parking = 158,
		RvPark = 158,
		Pharmacy = 165,
		PicnicSite = 168,
		PlaceOfWorship = 170,
		Church = 170,
		HinduTemple = 170,
		MosqueplaceOfWorship = 170,
		Synagogue = 170,
		Police = 172,
		PostOffice = 174,
		SportsCentre = 191,
		Supermarket = 199,
		GroceryOrSupermarket = 199,
		Taxi = 203,
		TaxiStand = 203,
		Theatre = 207,
		TownSquare = 212,
		Track = 215,
		TramStop = 216,
		University = 218,
		Viewpoint = 222,
		Terminal = 351,
		GolfCourse = 353,
		Fishing = 354,
		IceRink = 355,
		Dance = 356,
		SwimmingPool = 357,
		Bbq = 359,
		Administrative = 368,
		Veterinary = 220,
		VeterinaryCare = 220,
		Video = 221,
		MovieRental = 221,
		Wine = 231,
		Tobacco = 209,
		Toys = 214,
		TravelAgency = 217,
		Gate = 349,
		Architect = 369,
		NursingHome = 361,
		CommunityCenter = 362,
		SocialCenter = 363,
		Stripclub = 364,
		Boutique = 372,
		CarParts = 373,
		Charity = 375,
		BicycleParking = 392,
		Accessoires = 30,
		Adult = 31,
		Alcohol = 33,
		LiquorStore = 33,
		Antiques = 34,
		Art = 35,
		ArtsCentre = 36,
		Artwork = 37,
		Baby = 40,
		Bakery = 41,
		Bank = 42,
		Bar = 43,
		Beauty = 44,
		Beds = 45,
		Beverages = 46,
		Bicycle = 47,
		BicycleRental = 48,
		Biergarten = 49,
		BoatRental = 50,
		Books = 51,
		Brothel = 52,
		BureauDeChange = 53,
		Butcher = 55,
		Cafe = 56,
		Candy = 58,
		Car = 59,
		CarDealer = 59,
		CarRepair = 61,
		CarWash = 63,
		Casino = 64,
		Ceramics = 65,
		Chemist = 66,
		Church2 = 67,
		Clock = 70,
		Clocks = 71,
		Clothes = 72,
		ClothingStore = 72,
		Club = 73,
		Coffee = 74,
		Computer = 76,
		Confectionary = 78,
		Confectionery = 79,
		Convenience = 80,
		Copyshop = 81,
		Courthouse = 83,
		CoworkingSpace = 84,
		Deli = 85,
		Dentist = 86,
		DepartmentStore = 87,
		Doctors = 88,
		Doctor = 88,
		Doityourself = 89,
		Drugstore = 91,
		DryCleaning = 92,
		Electronics = 93,
		ElectronicsStore = 93,
		Fashion = 95,
		FastFood = 96,
		FireStation = 98,
		Fish = 99,
		FitnessCentre = 100,
		Florist = 101,
		Flowers = 102,
		Furniture = 106,
		Gallery = 107,
		ArtGallery = 107,
		Games = 108,
		GardenCentre = 110,
		General = 111,
		Gift = 112,
		Gifts = 113,
		Greengrocer = 115,
		Groceries = 116,
		GuestHouse = 117,
		Hairdresser = 118,
		BeautySalon = 118,
		HairCare = 118,
		Health = 118,
		Physiotherapist = 118,
		Halt = 119,
		Hardware = 121,
		FurnitureStore = 121,
		HardwareStore = 121,
		HomeGoodsStore = 121,
		HearingAids = 122,
		Hifi = 123,
		Hostel = 125,
		IceCream = 127,
		Insurance = 129,
		Jewelry = 130,
		Kindergarten = 131,
		Kiosk = 132,
		Kitchen = 133,
		Laundry = 134,
		Massage = 138,
		MiniatureGolf = 139,
		MobilePhone = 140,
		Motel = 141,
		Lodging = 141,
		Motorcycle = 142,
		MotorcycleRepair = 143,
		Music = 145,
		Newsagent = 147,
		Nightclub = 148,
		NightClub = 148,
		Office = 149,
		OfficeSupplies = 150,
		Optician = 151,
		Organic = 152,
		Orthopedics = 153,
		Outdoor = 154,
		Parfume = 156,
		Parquet = 159,
		Perfumery = 160,
		Pet = 161,
		PetShop = 162,
		PetStore = 162,
		PetSupply = 163,
		Petshop = 164,
		Phone = 166,
		Photo = 167,
		Pitch = 169,
		Playground = 171,
		PostBox = 173,
		Pub = 176,
		PublicBuilding = 177,
		Restaurant = 179,
		Food2 = 179,
		MealDelivery = 179,
		MealTakeaway = 179,
		Sauna = 180,
		Spa = 180,
		School = 181,
		ScubaDiving = 182,
		SecondHand = 183,
		Shoemaker = 186,
		Shoes = 187,
		ShoeStore = 187,
		Shop = 188,
		Slipway = 189,
		Sports = 190,
		BowlingAlley = 190,
		Gym = 190,
		Stationery = 195,
		Studio = 196,
		SubwayEntrance = 198,
		SubwayStation = 198,
		Tailoring = 200,
		Tanning = 201,
		Tattoo = 202,
		Tea = 204,
		Telecommunication = 205,
		Building = 393,
		Houseno = 313,
		Atm = 38,
		DrinkingWater = 90,
		ParcelBox = 155,
		Recycling = 178,
		Service = 184,
		Telephone = 206,
		Toilets = 210,
		VendingMachine = 219,
		BusStop = 395,
		VarietyStore = 396,
		Tyres = 398,
		EstateAgent = 399,
		FuneralDirectors = 400,
		Chalet = 401,
		CaravanSite = 402,
		AlpineHut = 403,
		SocialFacility = 407,
		EmergencyPhone = 408,
		Marketplace = 409,
		ParkingEntrance = 410,
		DrivingSchool = 411,
		HorseRiding = 412,
		Peak = 413,
		Continent = 415
	}

	public enum SKPOIMainCategory {
		Food = 1,
		Health = 2,
		Leisure = 3,
		Nightlife = 4,
		Public = 5,
		Services = 6,
		Shopping = 7,
		Accomodation = 8,
		Transport = 9
	}

	public enum SKAttributionPosition {
		None = 0,
		TopLeft = 1,
		TopMiddle = 2,
		TopRight = 3,
		BottomLeft = 4,
		BottomMiddle = 5,
		BottomRight = 6
	}

	public enum SKDrawingOrderType {
		AnnotationsOverDrawableObjects = 0,
		ableObjectsOverAnnotations = 1
	}
}
