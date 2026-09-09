package entity

import (
	"time"

	"wafer-movie/internal/domain/enum"

	"github.com/google/uuid"
	"gorm.io/gorm"
)

type User struct {
	Id                   uuid.UUID   `gorm:"column:Id;type:uuid;primaryKey" json:"id"`
	Name                 string      `gorm:"column:Name;type:varchar(63);not null" json:"name"`
	Gender               enum.Gender `gorm:"column:Gender;type:smallint;default:0" json:"gender"`
	AccountBalance       int         `gorm:"column:AccountBalance;type:integer;default:0" json:"accountBalance"`
	BirthDate            *time.Time  `gorm:"column:BirthDate;type:date" json:"birthDate"`
	UserName             string      `gorm:"column:UserName;type:varchar(63)" json:"userName"`
	NormalizedUserName   string      `gorm:"column:NormalizedUserName;type:varchar(63);uniqueIndex:UserNameIndex" json:"normalizedUserName"`
	Email                string      `gorm:"column:Email;type:varchar(127)" json:"email"`
	NormalizedEmail      string      `gorm:"column:NormalizedEmail;type:varchar(127);index:EmailIndex" json:"normalizedEmail"`
	EmailConfirmed       bool        `gorm:"column:EmailConfirmed;type:boolean;default:false" json:"emailConfirmed"`
	PasswordHash         string      `gorm:"column:PasswordHash;type:varchar(255)" json:"-"`
	SecurityStamp        string      `gorm:"column:SecurityStamp;type:varchar(255)" json:"-"`
	ConcurrencyStamp     string      `gorm:"column:ConcurrencyStamp;type:varchar(255)" json:"-"`
	PhoneNumber          string      `gorm:"column:PhoneNumber;type:varchar(15)" json:"phoneNumber"`
	PhoneNumberConfirmed bool        `gorm:"column:PhoneNumberConfirmed;type:boolean;default:false" json:"phoneNumberConfirmed"`
	TwoFactorEnabled     bool        `gorm:"column:TwoFactorEnabled;type:boolean;default:false" json:"twoFactorEnabled"`
	LockoutEnd           *time.Time  `gorm:"column:LockoutEnd;type:timestamptz" json:"lockoutEnd"`
	LockoutEnabled       bool        `gorm:"column:LockoutEnabled;type:boolean;default:false" json:"lockoutEnabled"`
	AccessFailedCount    int         `gorm:"column:AccessFailedCount;type:integer;default:0" json:"accessFailedCount"`

	Sessions   []UserSession `gorm:"foreignKey:UserId;references:Id;constraint:OnDelete:CASCADE" json:"sessions,omitempty"`
	MovieRates []MovieRate   `gorm:"foreignKey:UserId;references:Id;constraint:OnDelete:CASCADE" json:"movieRates,omitempty"`
	SerieRates []SerieRate   `gorm:"foreignKey:UserId;references:Id;constraint:OnDelete:CASCADE" json:"serieRates,omitempty"`
}

func (User) TableName() string {
	return "Users"
}

func (u *User) BeforeCreate(tx *gorm.DB) error {
	if u.Id == uuid.Nil {
		newId, err := uuid.NewV7()
		if err != nil {
			newId = uuid.New()
		}
		u.Id = newId
	}
	return nil
}

type UserSession struct {
	Id           uuid.UUID        `gorm:"column:Id;type:uuid;primaryKey" json:"id"`
	UserId       uuid.UUID        `gorm:"column:UserId;type:uuid;not null;index" json:"userId"`
	CreationDate time.Time        `gorm:"column:CreationDate;type:timestamp;not null" json:"creationDate"`
	IsActive     bool             `gorm:"column:IsActive;type:boolean;default:true" json:"isActive"`
	IpAddress    string           `gorm:"column:IpAddress;type:varchar(45);not null" json:"ipAddress"`
	DeviceKind   enum.DeviceKind  `gorm:"column:DeviceKind;type:smallint;default:0" json:"deviceKind"`
	DeviceOs     enum.DeviceOs    `gorm:"column:DeviceOs;type:smallint;default:0" json:"deviceOs"`
	DeviceAgent  enum.DeviceAgent `gorm:"column:DeviceAgent;type:smallint;default:0" json:"deviceAgent"`

	User *User `gorm:"foreignKey:UserId;references:Id" json:"user,omitempty"`
}

func (UserSession) TableName() string {
	return "UserSessions"
}

func (s *UserSession) BeforeCreate(tx *gorm.DB) error {
	if s.Id == uuid.Nil {
		newId, err := uuid.NewV7()
		if err != nil {
			newId = uuid.New()
		}
		s.Id = newId
	}
	if s.CreationDate.IsZero() {
		s.CreationDate = time.Now().UTC()
	}
	return nil
}

type Role struct {
	Id               uuid.UUID `gorm:"column:Id;type:uuid;primaryKey" json:"id"`
	Description      string    `gorm:"column:Description;type:text" json:"description"`
	Name             string    `gorm:"column:Name;type:varchar(256)" json:"name"`
	NormalizedName   string    `gorm:"column:NormalizedName;type:varchar(256);uniqueIndex:RoleNameIndex" json:"normalizedName"`
	ConcurrencyStamp string    `gorm:"column:ConcurrencyStamp;type:text" json:"concurrencyStamp"`
}

func (Role) TableName() string {
	return "Roles"
}

type UserRole struct {
	UserId uuid.UUID `gorm:"column:UserId;type:uuid;primaryKey" json:"userId"`
	RoleId uuid.UUID `gorm:"column:RoleId;type:uuid;primaryKey" json:"roleId"`
}

func (UserRole) TableName() string {
	return "UserRoles"
}

type Movie struct {
	Id             uuid.UUID                `gorm:"column:Id;type:uuid;primaryKey" json:"id"`
	IMDB           string                   `gorm:"column:IMDB;type:varchar(20);not null;uniqueIndex" json:"imdb"`
	Title          string                   `gorm:"column:Title;type:varchar(100);not null" json:"title"`
	Description    string                   `gorm:"column:Description;type:varchar(500);not null" json:"description"`
	Unavailable    bool                     `gorm:"column:Unavailable;type:boolean;default:false" json:"unavailable"`
	Length         int                      `gorm:"column:Length;type:integer;not null" json:"length"`
	IsFree         bool                     `gorm:"column:IsFree;type:boolean;default:false" json:"isFree"`
	OutYear        int                      `gorm:"column:OutYear;type:integer;not null" json:"outYear"`
	AgeRestriction enum.MovieAgeRestriction `gorm:"column:AgeRestriction;type:smallint;not null" json:"ageRestriction"`

	DownloadLinks []MovieDownloadLink `gorm:"foreignKey:MovieId;references:Id;constraint:OnDelete:CASCADE" json:"downloadLinks,omitempty"`
	Groups        []Group             `gorm:"many2many:MovieGroups;foreignKey:Id;joinForeignKey:MovieId;References:Id;joinReferences:GroupId" json:"groups,omitempty"`
	Rates         []MovieRate         `gorm:"foreignKey:MovieId;references:Id;constraint:OnDelete:CASCADE" json:"rates,omitempty"`
}

func (Movie) TableName() string {
	return "Movies"
}

func (m *Movie) BeforeCreate(tx *gorm.DB) error {
	if m.Id == uuid.Nil {
		newId, err := uuid.NewV7()
		if err != nil {
			newId = uuid.New()
		}
		m.Id = newId
	}
	return nil
}

type MovieRate struct {
	MovieId uuid.UUID `gorm:"column:MovieId;type:uuid;primaryKey" json:"movieId"`
	UserId  uuid.UUID `gorm:"column:UserId;type:uuid;primaryKey" json:"userId"`
	Score   uint8     `gorm:"column:Score;type:smallint;not null" json:"score"`

	Movie *Movie `gorm:"foreignKey:MovieId;references:Id;constraint:OnDelete:CASCADE" json:"-"`
	User  *User  `gorm:"foreignKey:UserId;references:Id;constraint:OnDelete:CASCADE" json:"-"`
}

func (MovieRate) TableName() string {
	return "MovieRates"
}

type MovieDownloadLink struct {
	Id                 uuid.UUID `gorm:"column:Id;type:uuid;primaryKey" json:"id"`
	MovieId            uuid.UUID `gorm:"column:MovieId;type:uuid;not null;index" json:"movieId"`
	Quality            string    `gorm:"column:Quality;type:text;not null" json:"quality"`
	Encoder            *string   `gorm:"column:Encoder;type:text" json:"encoder,omitempty"`
	Link               string    `gorm:"column:Link;type:text;not null" json:"link"`
	QualityExampleLink *string   `gorm:"column:QualityExampleLink;type:text" json:"qualityExampleLink,omitempty"`
	Dubbed             bool      `gorm:"column:Dubbed;type:boolean;default:false" json:"dubbed"`
	Size               *string   `gorm:"column:Size;type:text" json:"size,omitempty"`

	Movie *Movie `gorm:"foreignKey:MovieId;references:Id" json:"-"`
}

func (MovieDownloadLink) TableName() string {
	return "MovieDownloadLinks"
}

func (l *MovieDownloadLink) BeforeCreate(tx *gorm.DB) error {
	if l.Id == uuid.Nil {
		newId, err := uuid.NewV7()
		if err != nil {
			newId = uuid.New()
		}
		l.Id = newId
	}
	return nil
}

type Serie struct {
	Id              uuid.UUID                `gorm:"column:Id;type:uuid;primaryKey" json:"id"`
	IMDB            string                   `gorm:"column:IMDB;type:varchar(20);not null;uniqueIndex" json:"imdb"`
	Title           string                   `gorm:"column:Title;type:varchar(100);not null" json:"title"`
	Description     string                   `gorm:"column:Description;type:varchar(500);not null" json:"description"`
	Length          int                      `gorm:"column:Length;type:integer;not null" json:"length"`
	IsFree          bool                     `gorm:"column:IsFree;type:boolean;default:false" json:"isFree"`
	Unavailable     bool                     `gorm:"column:Unavailable;type:boolean;default:false" json:"unavailable"`
	StreamNetwork   *string                  `gorm:"column:StreamNetwork;type:text" json:"streamNetwork,omitempty"`
	FirstSeasonYear int                      `gorm:"column:FirstSeasonYear;type:integer;not null" json:"firstSeasonYear"`
	LastSeasonYear  *int                     `gorm:"column:LastSeasonYear;type:integer" json:"lastSeasonYear,omitempty"`
	AgeRestriction  enum.SerieAgeRestriction `gorm:"column:AgeRestriction;type:smallint;not null" json:"ageRestriction"`
	LastEpisodeDate *time.Time               `gorm:"column:LastEpisodeDate;type:timestamp" json:"lastEpisodeDate,omitempty"`

	Seasons []Season    `gorm:"foreignKey:SerieId;references:Id;constraint:OnDelete:CASCADE" json:"seasons,omitempty"`
	Groups  []Group     `gorm:"many2many:SerieGroups;foreignKey:Id;joinForeignKey:SerieId;References:Id;joinReferences:GroupId" json:"groups,omitempty"`
	Rates   []SerieRate `gorm:"foreignKey:SerieId;references:Id;constraint:OnDelete:CASCADE" json:"rates,omitempty"`
}

func (Serie) TableName() string {
	return "Series"
}

func (s *Serie) BeforeCreate(tx *gorm.DB) error {
	if s.Id == uuid.Nil {
		newId, err := uuid.NewV7()
		if err != nil {
			newId = uuid.New()
		}
		s.Id = newId
	}
	return nil
}

type Season struct {
	Id           uuid.UUID `gorm:"column:Id;type:uuid;primaryKey" json:"id"`
	SerieId      uuid.UUID `gorm:"column:SerieId;type:uuid;not null;index" json:"serieId"`
	Quality      *string   `gorm:"column:Quality;type:text" json:"quality,omitempty"`
	SeasonNumber int       `gorm:"column:SeasonNumber;type:integer;not null" json:"seasonNumber"`
	AverageSize  *string   `gorm:"column:AverageSize;type:text" json:"averageSize,omitempty"`
	IsLastSeason bool      `gorm:"column:IsLastSeason;type:boolean;default:false" json:"isLastSeason"`

	Episodes []Episode `gorm:"foreignKey:SeasonId;references:Id;constraint:OnDelete:CASCADE" json:"episodes,omitempty"`
	Serie    *Serie    `gorm:"foreignKey:SerieId;references:Id" json:"-"`
}

func (Season) TableName() string {
	return "Seasons"
}

func (s *Season) BeforeCreate(tx *gorm.DB) error {
	if s.Id == uuid.Nil {
		newId, err := uuid.NewV7()
		if err != nil {
			newId = uuid.New()
		}
		s.Id = newId
	}
	return nil
}

type Episode struct {
	Id            uuid.UUID `gorm:"column:Id;type:uuid;primaryKey" json:"id"`
	SeasonId      uuid.UUID `gorm:"column:SeasonId;type:uuid;not null;index" json:"seasonId"`
	EpisodeNumber int       `gorm:"column:EpisodeNumber;type:integer;not null" json:"episodeNumber"`
	IsLastEpisode bool      `gorm:"column:IsLastEpisode;type:boolean;default:false" json:"isLastEpisode"`

	DownloadLinks []SerieDownloadLink `gorm:"foreignKey:EpisodeId;references:Id;constraint:OnDelete:CASCADE" json:"downloadLinks,omitempty"`
	Season        *Season             `gorm:"foreignKey:SeasonId;references:Id" json:"-"`
}

func (Episode) TableName() string {
	return "Episodes"
}

func (e *Episode) BeforeCreate(tx *gorm.DB) error {
	if e.Id == uuid.Nil {
		newId, err := uuid.NewV7()
		if err != nil {
			newId = uuid.New()
		}
		e.Id = newId
	}
	return nil
}

type SerieDownloadLink struct {
	Id        uuid.UUID `gorm:"column:Id;type:uuid;primaryKey" json:"id"`
	EpisodeId uuid.UUID `gorm:"column:EpisodeId;type:uuid;not null;index" json:"episodeId"`
	Quality   string    `gorm:"column:Quality;type:text;not null" json:"quality"`
	Encoder   *string   `gorm:"column:Encoder;type:text" json:"encoder,omitempty"`
	Link      string    `gorm:"column:Link;type:text;not null" json:"link"`
	Dubbed    bool      `gorm:"column:Dubbed;type:boolean;default:false" json:"dubbed"`
	Size      *string   `gorm:"column:Size;type:text" json:"size,omitempty"`

	Episode *Episode `gorm:"foreignKey:EpisodeId;references:Id" json:"-"`
}

func (SerieDownloadLink) TableName() string {
	return "SerieDownloadLinks"
}

func (l *SerieDownloadLink) BeforeCreate(tx *gorm.DB) error {
	if l.Id == uuid.Nil {
		newId, err := uuid.NewV7()
		if err != nil {
			newId = uuid.New()
		}
		l.Id = newId
	}
	return nil
}

type SerieRate struct {
	SerieId uuid.UUID `gorm:"column:SerieId;type:uuid;primaryKey" json:"serieId"`
	UserId  uuid.UUID `gorm:"column:UserId;type:uuid;primaryKey" json:"userId"`
	Score   uint8     `gorm:"column:Score;type:smallint;not null" json:"score"`

	Serie *Serie `gorm:"foreignKey:SerieId;references:Id;constraint:OnDelete:CASCADE" json:"-"`
	User  *User  `gorm:"foreignKey:UserId;references:Id;constraint:OnDelete:CASCADE" json:"-"`
}

func (SerieRate) TableName() string {
	return "SerieRates"
}

type Group struct {
	Id                uuid.UUID `gorm:"column:Id;type:uuid;primaryKey" json:"id"`
	Name              string    `gorm:"column:Name;type:varchar(100);not null" json:"name"`
	Description       string    `gorm:"column:Description;type:varchar(500);not null" json:"description"`
	ImageUrl          *string   `gorm:"column:ImageUrl;type:text" json:"imageUrl,omitempty"`
	ImageThumbnailUrl *string   `gorm:"column:ImageThumbnailUrl;type:text" json:"imageThumbnailUrl,omitempty"`
	IsPublic          bool      `gorm:"column:IsPublic;type:boolean;default:false" json:"isPublic"`
	IsDeleted         bool      `gorm:"column:IsDeleted;type:boolean;default:false" json:"isDeleted"`

	Movies []Movie `gorm:"many2many:MovieGroups;foreignKey:Id;joinForeignKey:GroupId;References:Id;joinReferences:MovieId" json:"movies,omitempty"`
	Series []Serie `gorm:"many2many:SerieGroups;foreignKey:Id;joinForeignKey:GroupId;References:Id;joinReferences:SerieId" json:"series,omitempty"`
}

func (Group) TableName() string {
	return "Groups"
}

func (g *Group) BeforeCreate(tx *gorm.DB) error {
	if g.Id == uuid.Nil {
		newId, err := uuid.NewV7()
		if err != nil {
			newId = uuid.New()
		}
		g.Id = newId
	}
	return nil
}

type MovieGroup struct {
	GroupId uuid.UUID `gorm:"column:GroupId;type:uuid;primaryKey" json:"groupId"`
	MovieId uuid.UUID `gorm:"column:MovieId;type:uuid;primaryKey" json:"movieId"`
}

func (MovieGroup) TableName() string {
	return "MovieGroups"
}

type SerieGroup struct {
	GroupId uuid.UUID `gorm:"column:GroupId;type:uuid;primaryKey" json:"groupId"`
	SerieId uuid.UUID `gorm:"column:SerieId;type:uuid;primaryKey" json:"serieId"`
}

func (SerieGroup) TableName() string {
	return "SerieGroups"
}

type Genre struct {
	Id    uuid.UUID `gorm:"column:Id;type:uuid;primaryKey" json:"id"`
	Title string    `gorm:"column:Title;type:varchar(100);not null;uniqueIndex" json:"title"`
}

func (Genre) TableName() string {
	return "Genres"
}

func (g *Genre) BeforeCreate(tx *gorm.DB) error {
	if g.Id == uuid.Nil {
		newId, err := uuid.NewV7()
		if err != nil {
			newId = uuid.New()
		}
		g.Id = newId
	}
	return nil
}
