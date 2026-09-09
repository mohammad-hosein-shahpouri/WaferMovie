package i18n

import (
	"fmt"
	"strings"
)

type Language string

const (
	EnUS Language = "en-US"
	FaIR Language = "fa-IR"
)

var sharedMessages = map[Language]map[string]string{
	EnUS: {
		"Success":      "Success",
		"Failure":      "Failure",
		"Unauthorized": "Unauthorized",
		"Forbidden":    "Forbidden",
		"NotFound":     "Not Found",
		"InvalidData":  "Invalid Data",
		"Error":        "Internal Server Error",
	},
	FaIR: {
		"Success":      "عملیات با موفقیت انجام شد",
		"Failure":      "عملیات ناموفق بود",
		"Unauthorized": "دسترسی غیر مجاز",
		"Forbidden":    "دسترسی ممنوع است",
		"NotFound":     "موردی یافت نشد",
		"InvalidData":  "اطلاعات وارد شده نامعتبر است",
		"Error":        "خطایی در سرور رخ داده است",
	},
}

var validationMessages = map[Language]map[string]string{
	EnUS: {
		"{0} is required":  "{0} is required",
		"{0} is invalid":   "{0} is invalid",
		"{0} is not found": "{0} is not found",
		"{0} can not be longer than {1} characters":           "{0} cannot be longer than {1} characters",
		"{0} must be between {1} and {2}":                     "{0} must be between {1} and {2}",
		"No {0} found with this {1}":                          "No {0} found with this {1}",
		"{0} must contain at least one letter and one number": "{0} must contain at least one letter and one number",
		"{0} must match {1}":                                  "{0} must match {1}",
		"{0} already exists":                                  "{0} already exists",
	},
	FaIR: {
		"{0} is required":  "{0} الزامی است",
		"{0} is invalid":   "{0} نامعتبر است",
		"{0} is not found": "{0} یافت نشد",
		"{0} can not be longer than {1} characters":           "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد",
		"{0} must be between {1} and {2}":                     "{0} باید بین {1} و {2} باشد",
		"No {0} found with this {1}":                          "هیچ {0} با این {1} یافت نشد",
		"{0} must contain at least one letter and one number": "{0} باید حداقل شامل یک حرف و یک عدد باشد",
		"{0} must match {1}":                                  "{0} باید با {1} همخوانی داشته باشد",
		"{0} already exists":                                  "{0} از قبل وجود دارد",
	},
}

var propertyMessages = map[Language]map[string]string{
	EnUS: {
		"Title":                "Title",
		"Description":          "Description",
		"IMDB":                 "IMDB",
		"Email":                "Email",
		"Password":             "Password",
		"PasswordConfirmation": "Password Confirmation",
		"Name":                 "Name",
		"UserName":             "Username",
		"PhoneNumber":          "Phone Number",
		"movie":                "movie",
		"serie":                "serie",
		"group":                "group",
		"user":                 "user",
		"Id":                   "Id",
	},
	FaIR: {
		"Title":                "عنوان",
		"Description":          "توضیحات",
		"IMDB":                 "IMDB",
		"Email":                "ایمیل",
		"Password":             "رمز عبور",
		"PasswordConfirmation": "تکرار رمز عبور",
		"Name":                 "نام",
		"UserName":             "نام کاربری",
		"PhoneNumber":          "شماره همراه",
		"movie":                "فیلم",
		"serie":                "سریال",
		"group":                "گروه",
		"user":                 "کاربر",
		"Id":                   "شناسه",
	},
}

type I18n struct {
	lang Language
}

func New(lang string) *I18n {
	l := EnUS
	if strings.EqualFold(lang, "fa-ir") || strings.HasPrefix(strings.ToLower(lang), "fa") {
		l = FaIR
	}
	return &I18n{lang: l}
}

func (i *I18n) Language() Language {
	return i.lang
}

func (i *I18n) Shared(key string) string {
	if m, ok := sharedMessages[i.lang][key]; ok {
		return m
	}
	if m, ok := sharedMessages[EnUS][key]; ok {
		return m
	}
	return key
}

func (i *I18n) Property(key string) string {
	if m, ok := propertyMessages[i.lang][key]; ok {
		return m
	}
	if m, ok := propertyMessages[EnUS][key]; ok {
		return m
	}
	return key
}

func (i *I18n) Validation(template string, args ...any) string {
	translatedTemplate := template
	if m, ok := validationMessages[i.lang][template]; ok {
		translatedTemplate = m
	} else if m, ok := validationMessages[EnUS][template]; ok {
		translatedTemplate = m
	}

	result := translatedTemplate
	for idx, arg := range args {
		placeholder := fmt.Sprintf("{%d}", idx)
		result = strings.ReplaceAll(result, placeholder, fmt.Sprintf("%v", arg))
	}
	return result
}
