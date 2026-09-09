package enum

type Gender uint8

const (
	GenderPreferNotToSay Gender = 0
	GenderMale           Gender = 1
	GenderFemale         Gender = 2
)

func (g Gender) String() string {
	switch g {
	case GenderMale:
		return "Male"
	case GenderFemale:
		return "Female"
	default:
		return "PreferNotToSay"
	}
}

type MovieAgeRestriction uint8

const (
	MovieAgeG    MovieAgeRestriction = 0
	MovieAgePG   MovieAgeRestriction = 1
	MovieAgePG13 MovieAgeRestriction = 2
	MovieAgeR    MovieAgeRestriction = 4
	MovieAgeNC17 MovieAgeRestriction = 8
)

func (m MovieAgeRestriction) String() string {
	switch m {
	case MovieAgeG:
		return "G"
	case MovieAgePG:
		return "PG"
	case MovieAgePG13:
		return "PG-13"
	case MovieAgeR:
		return "R"
	case MovieAgeNC17:
		return "NC-17"
	default:
		return "G"
	}
}

type SerieAgeRestriction uint8

const (
	SerieAgeTVY  SerieAgeRestriction = 0
	SerieAgeTVY7 SerieAgeRestriction = 1
	SerieAgeTVG  SerieAgeRestriction = 2
	SerieAgeTVPG SerieAgeRestriction = 4
	SerieAgeTV14 SerieAgeRestriction = 8
	SerieAgeTVMA SerieAgeRestriction = 16
)

func (s SerieAgeRestriction) String() string {
	switch s {
	case SerieAgeTVY:
		return "TV-Y"
	case SerieAgeTVY7:
		return "TV-Y7"
	case SerieAgeTVG:
		return "TV-G"
	case SerieAgeTVPG:
		return "TV-PG"
	case SerieAgeTV14:
		return "TV-14"
	case SerieAgeTVMA:
		return "TV-MA"
	default:
		return "TV-Y"
	}
}

type ComparisonOperator uint8

const (
	EqualTo            ComparisonOperator = 0
	NotEqualTo         ComparisonOperator = 1
	GreaterThan        ComparisonOperator = 2
	LessThan           ComparisonOperator = 3
	GreaterThanOrEqual ComparisonOperator = 4
	LessThanOrEqual    ComparisonOperator = 5
	Contains           ComparisonOperator = 6
	NotContains        ComparisonOperator = 7
	StartsWith         ComparisonOperator = 8
	EndsWith           ComparisonOperator = 9
)

func (o ComparisonOperator) String() string {
	switch o {
	case EqualTo:
		return "EqualTo"
	case NotEqualTo:
		return "NotEqualTo"
	case GreaterThan:
		return "GreaterThan"
	case LessThan:
		return "LessThan"
	case GreaterThanOrEqual:
		return "GreaterThanOrEqual"
	case LessThanOrEqual:
		return "LessThanOrEqual"
	case Contains:
		return "Contains"
	case NotContains:
		return "NotContains"
	case StartsWith:
		return "StartsWith"
	case EndsWith:
		return "EndsWith"
	default:
		return "EqualTo"
	}
}

type DataType int

const (
	DataTypeString               DataType = 0
	DataTypeNumber               DataType = 1
	DataTypeCommaSeparatedNumber DataType = 2
	DataTypeBoolean              DataType = 3
	DataTypeDateTime             DataType = 4
	DataTypeDate                 DataType = 5
	DataTypeTime                 DataType = 6
	DataTypeEnum                 DataType = 7
	DataTypeGuid                 DataType = 8
	DataTypePhoneNumber          DataType = 9
	DataTypeEmail                DataType = 10
)

type DeviceKind uint8

const (
	DeviceKindUnknown DeviceKind = 0
	DeviceKindTV      DeviceKind = 1
	DeviceKindPhone   DeviceKind = 2
	DeviceKindTablet  DeviceKind = 3
	DeviceKindDesktop DeviceKind = 4
)

type DeviceOs uint8

const (
	DeviceOsUnknown DeviceOs = 0
	DeviceOsLinux   DeviceOs = 1
	DeviceOsWindows DeviceOs = 2
	DeviceOsMacOS   DeviceOs = 3
	DeviceOsAndroid DeviceOs = 4
	DeviceOsiOS     DeviceOs = 5
)

type DeviceAgent int

const (
	DeviceAgentNativeApplication DeviceAgent = 0
	DeviceAgentChrome            DeviceAgent = 1
	DeviceAgentSafari            DeviceAgent = 2
	DeviceAgentFirefox           DeviceAgent = 3
)
