# Contributing Guidelines

## Overview
This project follows a designer-first workflow for WinForms UI development.

## WinForms Event Wiring Standard
- Prefer designer-generated event handlers for control events (for example, button click handlers).
- Avoid constructor-based event subscriptions for UI controls when an equivalent designer-generated handler is appropriate.
- Only use constructor/event-code wiring when required by a specific technical constraint and document the reason in code comments.

## UI Consistency
- Keep UserControl layout and control creation in designer files.
- Keep behavior and data logic in the corresponding code-behind class.

## General Coding Practices
- Keep code clear and focused.
- Use descriptive method names for event handlers and helper methods.
- Keep asynchronous UI operations safe and user-friendly.