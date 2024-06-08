-- define the xaml for the form

local xamlMain = [[
<DockPanel xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' LastChildFill='true'>
  <StackPanel DockPanel.Dock='top' Orientation="Horizontal" HorizontalAlignment='center' Height='60' VerticalAlignment='center'>
    <Button Content='Button 1' Name='btn1' Width='100' Height='30'  Style='{StaticResource CustomButtonStyle}'/>
    <Button Content='Button 2' Name='btn2' Width='100' Height='30' Tag='button 2 pressed' Style='{StaticResource CustomButtonStyle}'/>
    <Button Content='Button 3' Name='btn3' Width='100' Height='30' Tag='0' Style='{StaticResource CustomButtonStyle}'/>
    <Button Content='Button 4' Name='btn4' Width='100' Height='30' Tag='0' Style='{StaticResource CustomButtonStyle}'/>
    <Button Content='Toggle Btn 3' Name='btn5' Width='100' Height='30' Tag='Fred' Style='{StaticResource CustomButtonStyle}'/>
  </StackPanel>
  <StackPanel VerticalAlignment='center' DockPanel.Dock='top' HorizontalAlignment='center'>
    <CheckBox Content='Button 4 Enabler' Name='chk1' Height='30' IsChecked='true'/>
    <Label Content='fred' Name='lbl1'/>
  </StackPanel>
  <StackPanel VerticalAlignment='center' HorizontalAlignment='center'>
    <Slider Name='sld1' Value='20' Maximum='100' Minimum='0' Width='200' Height='30' VerticalContentAlignment='Center'/>
    <ProgressBar Name='pgb1' Width='200' Height='30' Value='50' Maximum='100'/>
    <RadioButton Name='rbt1' Content='Choice 1' Height='30' IsChecked='true' VerticalContentAlignment='Center'/>
    <RadioButton Name='rbt2' Content='Choice 2' Height='30' VerticalContentAlignment='Center'/>
    <RadioButton Name='rbt3' Content='Choice 3' Height='30' VerticalContentAlignment='Center'/>
    <Rectangle Name='rct1' Height='40' Width='40' Fill='Beige' Stroke='Black' />
  </StackPanel>
</DockPanel>
]]

-- tell the C# container to load the xaml and create the form
LoadXaml(xamlMain)

-- activate form
Activate()


-- all done now


-- define the event handlers

-- each control will have appropriate event handlers defined.
-- the tag variable is the object (usually string or int) held in the 'tag' property of the control in question
-- it's null by default but could be useful for passing simple state information
-- by convention right now we return a 0 to indicate done. Future versions may use this to pass back a value to the caller

 function btn1_click(tag)
   if tag==1 then
     SetButtonColor('btn1','Yellow')
     SetTag('btn1',0)
   else
     SetButtonColor('btn1','LightGreen')
     SetTag('btn1',1)
   end
   return 0
 end


function btn2_click(tag)
   SetButtonColor('btn2','Red')
   SetLabel('lbl1',tag)
   if (tag=='button 2 pressed') then
     SetTag('btn2','button 2 pressed again')
   end
   return 0
 end


 function btn3_click(tag)
   SetTag('btn3', tag+1)
   SetButtonCaption('btn3','Clicked '..tag+1)
   return 0
end
 

function btn4_click(tag)
   SetTag('btn4', tag+1)
   SetButtonCaption('btn4','Clicked '..tag+1)
   return 0
end


function btn5_click(tag)
   ToggleButtonEnabled('btn3')
   return 0
end


function chk1_checked(tag)
   SetButtonEnabled('btn4',1)
   return 0
end


function chk1_unchecked(tag)
   SetButtonEnabled('btn4',0)
   return 0
end

function rbt1_checked(tag)
   SetRectangleColor('rct1','Beige')
   return 0
end

function rbt2_checked(tag)
   SetRectangleColor('rct1','Blue')
   return 0
end

function rbt3_checked(tag)
   SetRectangleColor('rct1','Red')
   return 0
end

function sld1_valuechanged(tag)
   SetProgressBarValue('pgb1', tag)
   return 0
end
