---
title: ActionState&lt;T&gt;
description: Translates raw input to semantic actions.
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class ActionState&lt;T&gt;

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Input](index.md)  
Assembly: StardewUI.dll  

</div>

Translates raw input to semantic actions.

```cs
public class ActionState<T>
```

### Type Parameters

**`T`**  
The semantic action type.


**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ ActionState&lt;T&gt;

## Members

### Constructors

 | Name | Description |
| --- | --- |
| [ActionState&lt;T&gt;(ActionRepeat, Boolean)](#actionstatetactionrepeat-bool) | Translates raw input to semantic actions. | 

### Methods

 | Name | Description |
| --- | --- |
| [Bind(SButton, T, ActionRepeat, Boolean?)](#bindsbutton-t-actionrepeat-bool) | Binds an action to a single button. | 
| [Bind(IReadOnlyList&lt;SButton&gt;, T, ActionRepeat, Boolean)](#bindireadonlylistsbutton-t-actionrepeat-bool) | Binds an action to several individual buttons. | 
| [Bind(Keybind, T, ActionRepeat, Boolean?)](#bindkeybind-t-actionrepeat-bool) | Binds an action to a button combination. | 
| [Bind(IReadOnlyList&lt;Keybind&gt;, T, ActionRepeat, Boolean?)](#bindireadonlylistkeybind-t-actionrepeat-bool) | Binds an action to several button combinations. | 
| [Bind(KeybindList, T, ActionRepeat, Boolean?)](#bindkeybindlist-t-actionrepeat-bool) | Binds an action to all keybinds in a KeybindList. | 
| [GetControllerBindings(T)](#getcontrollerbindingst) | Gets all controller bindings associated with a given action. | 
| [GetCurrentActions()](#getcurrentactions) | Gets the actions that should be run right now, either because one of the triggering buttons/combinations was just pressed, or because it was held and is due to repeat. | 
| [GetKeyboardBindings(T)](#getkeyboardbindingst) | Gets all keyboard bindings associated with a given action. | 

## Details

### Constructors

#### ActionState&lt;T&gt;(ActionRepeat, bool)

Translates raw input to semantic actions.

```cs
public ActionState<T>(StardewUI.Input.ActionRepeat defaultRepeat, bool defaultSuppress);
```

##### Parameters

**`defaultRepeat`** &nbsp; [ActionRepeat](actionrepeat.md)  
Default repetition settings for registered actions that do not specify one.

**`defaultSuppress`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Whether registered actions should, by default, suppress the normal game behavior for any buttons in their active keybinds. Individual registrations can override this setting.

-----

### Methods

#### Bind(SButton, T, ActionRepeat, bool?)

Binds an action to a single button.

```cs
public StardewUI.Input.ActionState<T> Bind(StardewModdingAPI.SButton button, T action, StardewUI.Input.ActionRepeat repeat, bool? suppress);
```

##### Parameters

**`button`** &nbsp; SButton  
The bound button.

**`action`** &nbsp; T  
The action to activate.

**`repeat`** &nbsp; [ActionRepeat](actionrepeat.md)  
Repeat behavior for this binding, if not using the default setting.

**`suppress`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)>  
Input suppression behavior for this binding, if not using the default setting.

##### Returns

[ActionState&lt;T&gt;](actionstate-1.md)

  The current instance.

-----

#### Bind(IReadOnlyList&lt;SButton&gt;, T, ActionRepeat, bool)

Binds an action to several individual buttons.

```cs
public StardewUI.Input.ActionState<T> Bind(System.Collections.Generic.IReadOnlyList<StardewModdingAPI.SButton> buttons, T action, StardewUI.Input.ActionRepeat repeat, bool suppress);
```

##### Parameters

**`buttons`** &nbsp; [IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<SButton>  
List of buttons which will each trigger the associated `action`.

**`action`** &nbsp; T  
The action to activate.

**`repeat`** &nbsp; [ActionRepeat](actionrepeat.md)  
Repeat behavior for this binding, if not using the default setting.

**`suppress`** &nbsp; [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)  
Input suppression behavior for this binding, if not using the default setting.

##### Returns

[ActionState&lt;T&gt;](actionstate-1.md)

  The current instance.

##### Remarks

The `buttons` are not treated as a single keybind; each individual SButton will independently trigger the `action`.

-----

#### Bind(Keybind, T, ActionRepeat, bool?)

Binds an action to a button combination.

```cs
public StardewUI.Input.ActionState<T> Bind(StardewModdingAPI.Utilities.Keybind keybind, T action, StardewUI.Input.ActionRepeat repeat, bool? suppress);
```

##### Parameters

**`keybind`** &nbsp; Keybind  
Keybind containing buttons that must be simultaneously pressed.

**`action`** &nbsp; T  
The action to activate.

**`repeat`** &nbsp; [ActionRepeat](actionrepeat.md)  
Repeat behavior for this binding, if not using the default setting.

**`suppress`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)>  
Input suppression behavior for this binding, if not using the default setting.

##### Returns

[ActionState&lt;T&gt;](actionstate-1.md)

  The current instance.

-----

#### Bind(IReadOnlyList&lt;Keybind&gt;, T, ActionRepeat, bool?)

Binds an action to several button combinations.

```cs
public StardewUI.Input.ActionState<T> Bind(System.Collections.Generic.IReadOnlyList<StardewModdingAPI.Utilities.Keybind> keybinds, T action, StardewUI.Input.ActionRepeat repeat, bool? suppress);
```

##### Parameters

**`keybinds`** &nbsp; [IReadOnlyList](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1)<Keybind>  
List of keybinds each of whose button combinations will trigger the associated `action`.

**`action`** &nbsp; T  
The action to activate.

**`repeat`** &nbsp; [ActionRepeat](actionrepeat.md)  
Repeat behavior for this binding, if not using the default setting.

**`suppress`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)>  
Input suppression behavior for this binding, if not using the default setting.

##### Returns

[ActionState&lt;T&gt;](actionstate-1.md)

  The current instance.

-----

#### Bind(KeybindList, T, ActionRepeat, bool?)

Binds an action to all keybinds in a KeybindList.

```cs
public StardewUI.Input.ActionState<T> Bind(StardewModdingAPI.Utilities.KeybindList keybindList, T action, StardewUI.Input.ActionRepeat repeat, bool? suppress);
```

##### Parameters

**`keybindList`** &nbsp; KeybindList  
List of all keybinds that should trigger the `action`.

**`action`** &nbsp; T  
The action to activate.

**`repeat`** &nbsp; [ActionRepeat](actionrepeat.md)  
Repeat behavior for this binding, if not using the default setting.

**`suppress`** &nbsp; [Nullable](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)>  
Input suppression behavior for this binding, if not using the default setting.

##### Returns

[ActionState&lt;T&gt;](actionstate-1.md)

  The current instance.

-----

#### GetControllerBindings(T)

Gets all controller bindings associated with a given action.

```cs
public System.Collections.Generic.IEnumerable<StardewModdingAPI.Utilities.Keybind> GetControllerBindings(T action);
```

##### Parameters

**`action`** &nbsp; T  
The action to look up.

##### Returns

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<Keybind>

  A sequence of Keybind elements that perform the specified `action` and use at least one controller button.

-----

#### GetCurrentActions()

Gets the actions that should be run right now, either because one of the triggering buttons/combinations was just pressed, or because it was held and is due to repeat.

```cs
public System.Collections.Generic.IEnumerable<T> GetCurrentActions();
```

##### Returns

[IEnumerable&lt;T&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)

  Sequence of actions that should be handled this frame.

##### Remarks

Current actions reset on every frame regardless of whether the actions were "handled". Code that reads from `GetCurrentActions` should generally do so at the end of an update tick, e.g. in SMAPI's UpdateTicked event.

-----

#### GetKeyboardBindings(T)

Gets all keyboard bindings associated with a given action.

```cs
public System.Collections.Generic.IEnumerable<StardewModdingAPI.Utilities.Keybind> GetKeyboardBindings(T action);
```

##### Parameters

**`action`** &nbsp; T  
The action to look up.

##### Returns

[IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<Keybind>

  A sequence of Keybind elements that perform the specified `action` and use at least one keyboard key.

-----

