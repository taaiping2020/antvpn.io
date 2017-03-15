
function New-User {
  [CmdletBinding(SupportsShouldProcess=$True,ConfirmImpact='Low')]
  param
  (
    [Parameter(Mandatory=$True, HelpMessage='This should be AD samAccountName.')]
    [Alias('name')]
    [ValidateLength(3,30)]
    [string]$username,
    [Parameter(Mandatory=$True, HelpMessage='This should be AD accountPassword.')]
    [ValidateLength(3,50)]
    [string]$password,
    [string]$groupname,
    [bool]$enabled = $true,
    [bool]$allowDialIn = $false
  )

  begin {
  }

  process {
    $secpasswd = ConvertTo-SecureString $password -AsPlainText -Force;
    try {
       New-ADUser -Name $username -Enabled $enabled -OtherAttributes @{msnpallowdialin=$allowDialIn} -AccountPassword $secpasswd -PasswordNeverExpires $true;
      }
      catch [Microsoft.ActiveDirectory.Management.ADIdentityAlreadyExistsException] {
          throw;
      }

    if([string]::IsNullOrEmpty($groupname) -eq $false)
    {
        Add-GroupMember -username $username -groupname $groupname;
    }
  }
}


function Set-User {
  [CmdletBinding(SupportsShouldProcess=$True,ConfirmImpact='Low')]
  param
  (
    [Parameter(Mandatory=$True, HelpMessage='This should be AD samAccountName.')]
    [Alias('name')]
    [ValidateLength(3,30)]
    [string]$username,
    [string]$password,
    [string]$groupname,
    [System.Nullable``1[[bool]]]$enabled,
    [System.Nullable``1[[bool]]]$allowDialIn
  )

  begin {
        try{
            Get-ADUser $username
        }
        catch [Microsoft.ActiveDirectory.Management.ADIdentityNotFoundException] {
            throw;
        }
  }

  process {
        if([string]::IsNullOrEmpty($password) -eq $false)
        {
           Set-ADAccountPassword -Identity $username -Reset -NewPassword (ConvertTo-SecureString -AsPlainText $password -Force)
		   Set-ADAccountPassword -Identity $username -Reset -NewPassword (ConvertTo-SecureString -AsPlainText $password -Force)
		   Set-ADAccountPassword -Identity $username -Reset -NewPassword (ConvertTo-SecureString -AsPlainText $password -Force)
        }
        if($enabled -ne $null)
        {
           Set-ADUser $username -Enabled $enabled
        }
        if($allowDialIn -ne $null)
        {
           Set-ADUser $username -Replace @{msnpallowdialin=$allowDialIn}
        }

        if([string]::IsNullOrEmpty($groupname) -eq $false)
        {
           Add-GroupMember -username $username -groupname $groupname;
        }

  }
}

function New-Group {
  [CmdletBinding(SupportsShouldProcess=$True,ConfirmImpact='Low')]
  param
  (
    [Parameter(Mandatory=$True, HelpMessage='This should be AD groupName.')]
    [Alias('name')]
    [ValidateLength(3,30)]
    [string]$groupName
  )

  begin {
  }

  process {
      New-ADGroup -Name $groupName -GroupScope Global;
  }
}

function Add-GroupMember {
  param
  (
    [Parameter(Mandatory=$True, HelpMessage='This should be AD group.')]
    [ValidateLength(3,30)]
    [string]$groupname,
    [Parameter(Mandatory=$True, HelpMessage='This should be AD username.')]
    [Alias('name')]
    [ValidateLength(3,30)]
    [string]$username
  )

  begin {
      try {
          Get-ADGroup $groupName;
      }
      catch [Microsoft.ActiveDirectory.Management.ADIdentityNotFoundException] {
          New-Group($groupName);
      }
  }

  process {
      $user = Get-ADUser -Identity $username;
      $group = Get-ADGroup $groupName;
      Add-ADGroupMember $group -Members $user;
  }
}
