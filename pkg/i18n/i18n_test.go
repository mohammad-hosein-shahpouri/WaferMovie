package i18n

import (
	"testing"

	"github.com/stretchr/testify/assert"
)

func TestI18n(t *testing.T) {
	en := New("en-US")
	assert.Equal(t, EnUS, en.Language())
	assert.Equal(t, "Success", en.Shared("Success"))
	assert.Equal(t, "Title", en.Property("Title"))
	assert.Equal(t, "Title is required", en.Validation("{0} is required", en.Property("Title")))

	fa := New("fa-IR")
	assert.Equal(t, FaIR, fa.Language())
	assert.Equal(t, "عملیات با موفقیت انجام شد", fa.Shared("Success"))
	assert.Equal(t, "عنوان", fa.Property("Title"))
	assert.Equal(t, "عنوان الزامی است", fa.Validation("{0} is required", fa.Property("Title")))
}
