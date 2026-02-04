using ElementMcpServer.Models;

namespace ElementMcpServer.Data;

/// <summary>
/// Provides access to Availity Element Design System documentation data.
/// </summary>
public class ElementDataService
{
    private readonly List<Component> _components;
    private readonly List<Foundation> _foundations;
    private readonly List<Pattern> _patterns;
    private readonly List<Template> _templates;

    public ElementDataService()
    {
        _components = InitializeComponents();
        _foundations = InitializeFoundations();
        _patterns = InitializePatterns();
        _templates = InitializeTemplates();
    }

    public IEnumerable<Component> GetAllComponents() => _components;
    public Component? GetComponent(string id) => _components.FirstOrDefault(c => c.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
    public IEnumerable<Component> GetComponentsByCategory(string category) => 
        _components.Where(c => c.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<Foundation> GetAllFoundations() => _foundations;
    public Foundation? GetFoundation(string id) => _foundations.FirstOrDefault(f => f.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
    public IEnumerable<Foundation> GetFoundationsByType(string type) => 
        _foundations.Where(f => f.Type.Equals(type, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<Pattern> GetAllPatterns() => _patterns;
    public Pattern? GetPattern(string id) => _patterns.FirstOrDefault(p => p.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
    public IEnumerable<Pattern> GetPatternsByCategory(string category) => 
        _patterns.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<Template> GetAllTemplates() => _templates;
    public Template? GetTemplate(string id) => _templates.FirstOrDefault(t => t.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
    public IEnumerable<Template> GetTemplatesByType(string type) => 
        _templates.Where(t => t.Type.Equals(type, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<object> Search(string query)
    {
        var results = new List<object>();
        var searchTerm = query.ToLower();

        results.AddRange(_components.Where(c => 
            c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
            c.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));

        results.AddRange(_foundations.Where(f => 
            f.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
            f.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));

        results.AddRange(_patterns.Where(p => 
            p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
            p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));

        results.AddRange(_templates.Where(t => 
            t.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
            t.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));

        return results;
    }

    private List<Component> InitializeComponents()
    {
        return new List<Component>
        {
            new Component
            {
                Id = "button",
                Name = "Button",
                Category = "Inputs",
                Description = "Buttons allow users to trigger actions and navigate throughout the application. Element provides Material UI Button with Availity theming.",
                Usage = "Import Button from @availity/element and use it to trigger actions. Buttons support various variants (text, contained, outlined) and colors.",
                Example = @"import { Button } from '@availity/element';

<Button variant=""contained"" color=""primary"">
  Click Me
</Button>",
                Props = new List<ComponentProp>
                {
                    new ComponentProp { Name = "variant", Type = "string", Description = "Button variant: text, contained, outlined", DefaultValue = "text" },
                    new ComponentProp { Name = "color", Type = "string", Description = "Button color: primary, secondary, error, info, success, warning", DefaultValue = "primary" },
                    new ComponentProp { Name = "disabled", Type = "boolean", Description = "If true, the button is disabled", DefaultValue = "false" },
                    new ComponentProp { Name = "size", Type = "string", Description = "Size of the button: small, medium, large", DefaultValue = "medium" },
                    new ComponentProp { Name = "onClick", Type = "function", Description = "Function called when button is clicked" }
                },
                Accessibility = "Buttons should have descriptive text or aria-label. Use proper color contrast and keyboard navigation support.",
                StorybookUrl = "https://availity.github.io/element/?path=/docs/components-button--docs",
                RelatedComponents = new List<string> { "IconButton", "ButtonGroup" }
            },
            new Component
            {
                Id = "card",
                Name = "Card",
                Category = "Surfaces",
                Description = "Cards contain content and actions about a single subject. They are versatile surfaces for displaying information.",
                Usage = "Use Card to group related information. Cards can include headers, content, actions, and media.",
                Example = @"import { Card, CardContent, CardHeader } from '@availity/element';

<Card>
  <CardHeader title=""Card Title"" />
  <CardContent>
    Card content goes here
  </CardContent>
</Card>",
                Props = new List<ComponentProp>
                {
                    new ComponentProp { Name = "elevation", Type = "number", Description = "Shadow depth (0-24)", DefaultValue = "1" },
                    new ComponentProp { Name = "variant", Type = "string", Description = "Card variant: elevation, outlined", DefaultValue = "elevation" }
                },
                StorybookUrl = "https://availity.github.io/element/?path=/docs/components-card--docs",
                RelatedComponents = new List<string> { "Paper", "Accordion" }
            },
            new Component
            {
                Id = "textfield",
                Name = "TextField",
                Category = "Inputs",
                Description = "Text fields let users enter and edit text. Element provides Material UI TextField with form validation support.",
                Usage = "Use TextField for text input with built-in validation, labels, and helper text support.",
                Example = @"import { TextField } from '@availity/element';

<TextField
  label=""Email""
  type=""email""
  required
  helperText=""Enter your email address""
/>",
                Props = new List<ComponentProp>
                {
                    new ComponentProp { Name = "label", Type = "string", Description = "Label text for the input" },
                    new ComponentProp { Name = "type", Type = "string", Description = "Input type (text, email, password, etc.)", DefaultValue = "text" },
                    new ComponentProp { Name = "required", Type = "boolean", Description = "If true, the field is required", DefaultValue = "false" },
                    new ComponentProp { Name = "error", Type = "boolean", Description = "If true, displays error state", DefaultValue = "false" },
                    new ComponentProp { Name = "helperText", Type = "string", Description = "Helper text displayed below input" },
                    new ComponentProp { Name = "disabled", Type = "boolean", Description = "If true, the input is disabled", DefaultValue = "false" }
                },
                Accessibility = "TextFields include proper labels, ARIA attributes, and keyboard navigation. Use helperText for additional instructions.",
                StorybookUrl = "https://availity.github.io/element/?path=/docs/form-components-textfield--docs",
                RelatedComponents = new List<string> { "Select", "Autocomplete", "DatePicker" }
            },
            new Component
            {
                Id = "alert",
                Name = "Alert",
                Category = "Feedback",
                Description = "Alerts display brief messages for users without interrupting their workflow.",
                Usage = "Use alerts to provide feedback about actions or communicate important information.",
                Example = @"import { Alert } from '@availity/element';

<Alert severity=""success"">
  Your changes have been saved successfully!
</Alert>",
                Props = new List<ComponentProp>
                {
                    new ComponentProp { Name = "severity", Type = "string", Description = "Alert severity: error, warning, info, success", DefaultValue = "info" },
                    new ComponentProp { Name = "variant", Type = "string", Description = "Alert variant: standard, filled, outlined", DefaultValue = "standard" },
                    new ComponentProp { Name = "onClose", Type = "function", Description = "Callback when close button is clicked" }
                },
                StorybookUrl = "https://availity.github.io/element/?path=/docs/components-alert--docs",
                RelatedComponents = new List<string> { "Snackbar", "Dialog" }
            },
            new Component
            {
                Id = "table",
                Name = "Table",
                Category = "Data Display",
                Description = "Tables display sets of data in rows and columns with sorting, filtering, and pagination capabilities.",
                Usage = "Use tables to organize and display structured data that users need to scan, compare, and analyze.",
                Example = @"import { Table, TableBody, TableCell, TableHead, TableRow } from '@availity/element';

<Table>
  <TableHead>
    <TableRow>
      <TableCell>Name</TableCell>
      <TableCell>Status</TableCell>
    </TableRow>
  </TableHead>
  <TableBody>
    <TableRow>
      <TableCell>John Doe</TableCell>
      <TableCell>Active</TableCell>
    </TableRow>
  </TableBody>
</Table>",
                Props = new List<ComponentProp>
                {
                    new ComponentProp { Name = "size", Type = "string", Description = "Table size: small, medium", DefaultValue = "medium" },
                    new ComponentProp { Name = "stickyHeader", Type = "boolean", Description = "If true, header stays fixed on scroll", DefaultValue = "false" }
                },
                StorybookUrl = "https://availity.github.io/element/?path=/docs/components-table--docs",
                RelatedComponents = new List<string> { "DataGrid", "List" }
            },
            new Component
            {
                Id = "dialog",
                Name = "Dialog",
                Category = "Feedback",
                Description = "Dialogs inform users about tasks and can contain critical information, require decisions, or involve multiple tasks.",
                Usage = "Use dialogs for important interactions that require user attention or decisions before proceeding.",
                Example = @"import { Dialog, DialogTitle, DialogContent, DialogActions, Button } from '@availity/element';

<Dialog open={open} onClose={handleClose}>
  <DialogTitle>Confirm Action</DialogTitle>
  <DialogContent>
    Are you sure you want to proceed?
  </DialogContent>
  <DialogActions>
    <Button onClick={handleClose}>Cancel</Button>
    <Button onClick={handleConfirm}>Confirm</Button>
  </DialogActions>
</Dialog>",
                Props = new List<ComponentProp>
                {
                    new ComponentProp { Name = "open", Type = "boolean", Required = true, Description = "If true, dialog is visible" },
                    new ComponentProp { Name = "onClose", Type = "function", Description = "Callback when dialog should close" },
                    new ComponentProp { Name = "maxWidth", Type = "string", Description = "Max width: xs, sm, md, lg, xl", DefaultValue = "sm" },
                    new ComponentProp { Name = "fullScreen", Type = "boolean", Description = "If true, dialog is full screen", DefaultValue = "false" }
                },
                Accessibility = "Dialogs trap focus, have proper ARIA roles, and support keyboard navigation including Escape to close.",
                StorybookUrl = "https://availity.github.io/element/?path=/docs/components-dialog--docs",
                RelatedComponents = new List<string> { "Alert", "Drawer", "Modal" }
            }
        };
    }

    private List<Foundation> InitializeFoundations()
    {
        return new List<Foundation>
        {
            new Foundation
            {
                Id = "colors",
                Name = "Colors",
                Type = "Color",
                Description = "Element uses a comprehensive color system built on Material Design principles with Availity-specific brand colors.",
                Usage = "Access colors through design tokens. Use primary colors for main actions, secondary for less prominent actions, and semantic colors (error, warning, info, success) for status indication.",
                Tokens = new Dictionary<string, string>
                {
                    { "primary.main", "#0078B6" },
                    { "primary.light", "#33A3D5" },
                    { "primary.dark", "#00578F" },
                    { "secondary.main", "#9C27B0" },
                    { "error.main", "#D32F2F" },
                    { "warning.main", "#ED6C02" },
                    { "info.main", "#0288D1" },
                    { "success.main", "#2E7D32" }
                },
                Example = @"import { useTheme } from '@availity/element';

const theme = useTheme();
const primaryColor = theme.palette.primary.main;",
                RelatedFoundations = new List<string> { "Typography", "Theme" }
            },
            new Foundation
            {
                Id = "typography",
                Name = "Typography",
                Type = "Typography",
                Description = "Element uses a type scale system based on Material Design with Availity's brand typography. The system includes various text styles for different contexts.",
                Usage = "Use Typography component or theme typography tokens to apply consistent text styles across your application.",
                Tokens = new Dictionary<string, string>
                {
                    { "fontFamily", "'Roboto', 'Helvetica', 'Arial', sans-serif" },
                    { "h1.fontSize", "96px" },
                    { "h2.fontSize", "60px" },
                    { "h3.fontSize", "48px" },
                    { "h4.fontSize", "34px" },
                    { "h5.fontSize", "24px" },
                    { "h6.fontSize", "20px" },
                    { "body1.fontSize", "16px" },
                    { "body2.fontSize", "14px" }
                },
                Example = @"import { Typography } from '@availity/element';

<Typography variant=""h1"">Heading 1</Typography>
<Typography variant=""body1"">Body text</Typography>",
                RelatedFoundations = new List<string> { "Colors", "Spacing" }
            },
            new Foundation
            {
                Id = "spacing",
                Name = "Spacing",
                Type = "Spacing",
                Description = "Element uses an 8px grid system for consistent spacing throughout the design system. The spacing scale helps maintain visual rhythm and hierarchy.",
                Usage = "Use theme spacing function or spacing tokens to apply consistent margins and padding. The base unit is 8px.",
                Tokens = new Dictionary<string, string>
                {
                    { "spacing(1)", "8px" },
                    { "spacing(2)", "16px" },
                    { "spacing(3)", "24px" },
                    { "spacing(4)", "32px" },
                    { "spacing(5)", "40px" },
                    { "spacing(6)", "48px" }
                },
                Example = @"import { Box, useTheme } from '@availity/element';

const theme = useTheme();
<Box sx={{ padding: theme.spacing(2), margin: theme.spacing(1) }}>
  Content with consistent spacing
</Box>",
                RelatedFoundations = new List<string> { "Layout", "Grid" }
            },
            new Foundation
            {
                Id = "elevation",
                Name = "Elevation",
                Type = "Elevation",
                Description = "Elevation represents the relative depth or distance between surfaces. Element uses shadows to communicate elevation levels from 0 to 24.",
                Usage = "Use elevation to create visual hierarchy and separate surfaces. Higher elevations appear closer to the user.",
                Tokens = new Dictionary<string, string>
                {
                    { "elevation1", "0px 2px 1px -1px rgba(0,0,0,0.2)" },
                    { "elevation2", "0px 3px 1px -2px rgba(0,0,0,0.2)" },
                    { "elevation3", "0px 3px 3px -2px rgba(0,0,0,0.2)" },
                    { "elevation4", "0px 2px 4px -1px rgba(0,0,0,0.2)" }
                },
                Example = @"import { Paper } from '@availity/element';

<Paper elevation={2}>
  Content with elevation
</Paper>",
                RelatedFoundations = new List<string> { "Colors", "Surfaces" }
            },
            new Foundation
            {
                Id = "theme",
                Name = "Theme",
                Type = "Theme",
                Description = "The ThemeProvider component applies Availity design tokens and enables consistent theming across your application. It wraps Material UI's ThemeProvider with Availity-specific customizations.",
                Usage = "Wrap your application root with ThemeProvider to apply Element theming. Access theme values using useTheme hook.",
                Example = @"import { ThemeProvider } from '@availity/element';

function App() {
  return (
    <ThemeProvider>
      <YourApp />
    </ThemeProvider>
  );
}",
                RelatedFoundations = new List<string> { "Colors", "Typography", "Spacing" }
            }
        };
    }

    private List<Pattern> InitializePatterns()
    {
        return new List<Pattern>
        {
            new Pattern
            {
                Id = "form-validation",
                Name = "Form Validation",
                Category = "Forms",
                Description = "A pattern for implementing consistent form validation across healthcare applications with real-time feedback and accessibility support.",
                Problem = "Forms need consistent validation logic, error messaging, and accessibility support across different healthcare workflows.",
                Solution = "Use Element's form components with built-in validation support, combining TextField, Select, and other inputs with validation libraries like Yup or Zod.",
                BestPractices = "Validate on blur for better UX, show clear error messages, use semantic colors for validation states, ensure keyboard navigation works properly.",
                Example = @"import { TextField, Button } from '@availity/element';
import { useForm } from 'react-hook-form';

function MyForm() {
  const { register, handleSubmit, formState: { errors } } = useForm();
  
  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <TextField
        {...register('email', { required: 'Email is required' })}
        label=""Email""
        error={!!errors.email}
        helperText={errors.email?.message}
      />
      <Button type=""submit"">Submit</Button>
    </form>
  );
}",
                ComponentsUsed = new List<string> { "TextField", "Button", "FormHelperText" },
                RelatedPatterns = new List<string> { "Error Handling", "Accessibility" }
            },
            new Pattern
            {
                Id = "data-loading",
                Name = "Data Loading States",
                Category = "Feedback",
                Description = "Pattern for managing and displaying loading, error, and empty states when fetching data.",
                Problem = "Users need clear feedback when data is being loaded, when errors occur, or when no data is available.",
                Solution = "Implement consistent loading indicators, error messages, and empty states using Element components like CircularProgress, Alert, and custom empty state components.",
                BestPractices = "Show loading state immediately, provide meaningful error messages, offer retry actions, use skeleton screens for better perceived performance.",
                Example = @"import { CircularProgress, Alert, Box } from '@availity/element';

function DataView({ loading, error, data }) {
  if (loading) return <CircularProgress />;
  if (error) return <Alert severity=""error"">{error.message}</Alert>;
  if (!data?.length) return <Box>No data available</Box>;
  
  return <div>{/* render data */}</div>;
}",
                ComponentsUsed = new List<string> { "CircularProgress", "Alert", "Skeleton" },
                RelatedPatterns = new List<string> { "Error Handling", "Progressive Disclosure" }
            },
            new Pattern
            {
                Id = "modal-workflow",
                Name = "Modal Workflow",
                Category = "Navigation",
                Description = "Pattern for implementing multi-step workflows within modal dialogs, commonly used for complex healthcare forms.",
                Problem = "Complex workflows need to be contained without leaving the current context, with clear progress indication and navigation.",
                Solution = "Use Dialog with Stepper components to create multi-step workflows, maintaining state and allowing forward/backward navigation.",
                BestPractices = "Show clear progress, allow cancellation, save draft state, validate each step before proceeding, provide summary before final submission.",
                Example = @"import { Dialog, Stepper, Step, StepLabel, Button } from '@availity/element';

function WorkflowDialog({ open, onClose }) {
  const [activeStep, setActiveStep] = useState(0);
  
  return (
    <Dialog open={open} onClose={onClose} maxWidth=""md"">
      <Stepper activeStep={activeStep}>
        <Step><StepLabel>Step 1</StepLabel></Step>
        <Step><StepLabel>Step 2</StepLabel></Step>
        <Step><StepLabel>Step 3</StepLabel></Step>
      </Stepper>
      {/* Step content */}
      <Button onClick={() => setActiveStep(prev => prev + 1)}>Next</Button>
    </Dialog>
  );
}",
                ComponentsUsed = new List<string> { "Dialog", "Stepper", "Button", "TextField" },
                RelatedPatterns = new List<string> { "Form Validation", "Navigation" }
            },
            new Pattern
            {
                Id = "search-filter",
                Name = "Search and Filter",
                Category = "Data Display",
                Description = "Pattern for implementing search and filtering functionality in data-heavy healthcare applications.",
                Problem = "Users need to quickly find specific information in large datasets like patient records or provider lists.",
                Solution = "Combine TextField for search input with Autocomplete, Select, and Chip components for filters, with debounced search and clear filter options.",
                BestPractices = "Debounce search input, show result counts, allow filter combinations, provide clear all filters action, persist filter state.",
                Example = @"import { TextField, Autocomplete, Chip } from '@availity/element';

function SearchAndFilter() {
  const [search, setSearch] = useState('');
  const [filters, setFilters] = useState([]);
  
  return (
    <>
      <TextField
        placeholder=""Search...""
        value={search}
        onChange={(e) => setSearch(e.target.value)}
      />
      <Autocomplete
        multiple
        options={filterOptions}
        renderTags={(value, getTagProps) =>
          value.map((option, index) => (
            <Chip label={option} {...getTagProps({ index })} />
          ))
        }
      />
    </>
  );
}",
                ComponentsUsed = new List<string> { "TextField", "Autocomplete", "Chip", "Select" },
                RelatedPatterns = new List<string> { "Data Loading States", "Pagination" }
            }
        };
    }

    private List<Template> InitializeTemplates()
    {
        return new List<Template>
        {
            new Template
            {
                Id = "dashboard",
                Name = "Dashboard Layout",
                Type = "Page Layout",
                Description = "A comprehensive dashboard template with navigation, header, and content areas optimized for healthcare applications displaying metrics and data visualizations.",
                UseCase = "Use this template for application dashboards that need to display multiple data widgets, charts, and key metrics at a glance.",
                Customization = "Customize grid layouts, add or remove widgets, adjust navigation menu items, and modify color schemes to match your application needs.",
                Example = @"import { Box, Grid, Card, AppBar, Toolbar, Typography } from '@availity/element';

function Dashboard() {
  return (
    <Box sx={{ display: 'flex' }}>
      <AppBar position=""fixed"">
        <Toolbar>
          <Typography variant=""h6"">Dashboard</Typography>
        </Toolbar>
      </AppBar>
      <Box component=""main"" sx={{ flexGrow: 1, p: 3 }}>
        <Grid container spacing={3}>
          <Grid item xs={12} md={6}>
            <Card>{/* Widget 1 */}</Card>
          </Grid>
          <Grid item xs={12} md={6}>
            <Card>{/* Widget 2 */}</Card>
          </Grid>
        </Grid>
      </Box>
    </Box>
  );
}",
                ComponentsUsed = new List<string> { "AppBar", "Toolbar", "Grid", "Card", "Typography" },
                PatternsUsed = new List<string> { "Data Loading States", "Search and Filter" },
                RelatedTemplates = new List<string> { "List View", "Detail View" }
            },
            new Template
            {
                Id = "form-page",
                Name = "Form Page",
                Type = "Form",
                Description = "A structured form template for collecting healthcare information with sections, validation, and responsive layout.",
                UseCase = "Use for patient intake forms, provider registration, claims submission, or any multi-field data entry workflow.",
                Customization = "Add or remove form sections, customize validation rules, adjust field types, and modify submit behavior.",
                Example = @"import { Box, Card, CardHeader, CardContent, TextField, Button, Grid } from '@availity/element';

function FormPage() {
  return (
    <Box sx={{ maxWidth: 960, mx: 'auto', p: 3 }}>
      <Card>
        <CardHeader title=""Patient Information"" />
        <CardContent>
          <Grid container spacing={2}>
            <Grid item xs={12} md={6}>
              <TextField fullWidth label=""First Name"" required />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField fullWidth label=""Last Name"" required />
            </Grid>
            <Grid item xs={12}>
              <Button variant=""contained"">Submit</Button>
            </Grid>
          </Grid>
        </CardContent>
      </Card>
    </Box>
  );
}",
                ComponentsUsed = new List<string> { "Card", "TextField", "Button", "Grid", "Select", "DatePicker" },
                PatternsUsed = new List<string> { "Form Validation", "Modal Workflow" },
                RelatedTemplates = new List<string> { "Dashboard Layout", "Detail View" }
            },
            new Template
            {
                Id = "list-view",
                Name = "List View",
                Type = "Data Display",
                Description = "A template for displaying searchable, filterable lists of items with pagination support, ideal for patient lists, provider directories, or claims records.",
                UseCase = "Use when displaying lists of entities that users need to search, filter, sort, and navigate through.",
                Customization = "Customize columns, add filters, adjust pagination settings, modify row actions, and add bulk actions.",
                Example = @"import { Box, TextField, Table, TableBody, TableCell, TableHead, TableRow, TablePagination } from '@availity/element';

function ListView() {
  return (
    <Box sx={{ p: 3 }}>
      <TextField placeholder=""Search..."" fullWidth sx={{ mb: 2 }} />
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>Name</TableCell>
            <TableCell>Status</TableCell>
            <TableCell>Date</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {/* rows */}
        </TableBody>
      </Table>
      <TablePagination />
    </Box>
  );
}",
                ComponentsUsed = new List<string> { "Table", "TextField", "TablePagination", "Chip", "IconButton" },
                PatternsUsed = new List<string> { "Search and Filter", "Data Loading States" },
                RelatedTemplates = new List<string> { "Dashboard Layout", "Detail View" }
            },
            new Template
            {
                Id = "detail-view",
                Name = "Detail View",
                Type = "Page Layout",
                Description = "A template for displaying detailed information about a single entity with tabs for organizing related information.",
                UseCase = "Use for patient details, provider profiles, claim details, or any entity that has extensive information to display.",
                Customization = "Add or remove tabs, customize information sections, add action buttons, modify layout density.",
                Example = @"import { Box, Card, CardHeader, CardContent, Tabs, Tab, Typography, Button } from '@availity/element';

function DetailView() {
  const [tab, setTab] = useState(0);
  
  return (
    <Box sx={{ p: 3 }}>
      <Card>
        <CardHeader 
          title=""Patient Details""
          action={<Button>Edit</Button>}
        />
        <Tabs value={tab} onChange={(e, v) => setTab(v)}>
          <Tab label=""Overview"" />
          <Tab label=""History"" />
          <Tab label=""Documents"" />
        </Tabs>
        <CardContent>
          {/* Tab content */}
        </CardContent>
      </Card>
    </Box>
  );
}",
                ComponentsUsed = new List<string> { "Card", "Tabs", "Typography", "Button", "List", "Divider" },
                PatternsUsed = new List<string> { "Data Loading States" },
                RelatedTemplates = new List<string> { "Dashboard Layout", "List View", "Form Page" }
            }
        };
    }
}
