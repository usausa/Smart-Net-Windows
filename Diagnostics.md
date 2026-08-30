# Diagnostics

| ID | Severity | Description | How to fix |
|---|---|---|---|
| SWD0001 | ❌ Error | `[DependencyProperty]` property is not declared as a partial property | Declare the property as `public partial T Name { get; set; }` |
| SWD0002 | ❌ Error | `[DependencyProperty]` property is static, and a static property can not be backed by an instance value | Remove `static` from the property, or register the `DependencyProperty` by hand |
| SWD0003 | ❌ Error | `[DependencyProperty]` property does not have both accessors, or an accessor has its own accessibility modifier such as `private set` | Declare the property as `{ get; set; }` without accessor modifiers |
| SWD0004 | ❌ Error | The type containing the `[DependencyProperty]` property, or one of its outer types, is not partial | Add `partial` to the containing type and to every outer type |
| SWD0005 | ❌ Error | The type containing the `[DependencyProperty]` property has an explicit base type that is not derived from `DependencyObject`, so `GetValue` and `SetValue` are not available. A type with no explicit base type is not checked, because the base type can be declared in another partial declaration such as one generated from XAML | Derive the containing type from `DependencyObject` |
| SWD0006 | ❌ Error | The type containing the `[DependencyProperty]` property is generic, and a static `DependencyProperty` field would be created per type argument | Move the property to a non generic type |
| SWD0007 | ❌ Error | `[DependencyProperty]` specifies both `DefaultValue` and `DefaultValueExpression`, and only one default value can be used | Remove either `DefaultValue` or `DefaultValueExpression` |
| SWD0008 | ❌ Error | The method specified for `PropertyChanged`, `Coerce` or `Validate` of `[DependencyProperty]` does not exist in the containing type | Specify the method with `nameof`, and define it in the same type |
| SWD0009 | ❌ Error | The signature of the callback method specified by `[DependencyProperty]` does not match, or more than one overload is applicable | Match the signature: `PropertyChanged` is `void ()` or `void (T oldValue, T newValue)`, `Coerce` is `T (T value)`, `Validate` is `static bool (T value)` |
| SWD0010 | ❌ Error | The value specified for `DefaultValue` of `[DependencyProperty]` can not be written as a constant in the generated code | Use `DefaultValueExpression` to give the default value as an expression |
