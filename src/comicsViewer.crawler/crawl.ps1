#$a = foreach {$_.Issues.value | foreach {$b = iwr $_.href; $b.links | where {$_.href -like "*blogspot*"} | foreach {$_.href}}}
$allComics = @();
$cc = 0;
$startTime = [System.DateTime]::Now;
$maxPageNum = 2106;
1..$maxPageNum |
    ForEach-Object {$cc++;$downLinks = 0; "http://viewcomic.com/page/" + $_ + "/"} | 
    ForEach-Object {Invoke-WebRequest $_} | 
    ForEach-Object {$_.Links} | 
    Where-Object innerhtml -like "*(*)*" |  
    ForEach-Object -Process {
        $nameAndIssue = $_.innerhtml.replace('.', '').split('(')[-20..-2] | 
            ForEach-Object -Begin {$a = ""} -Process { 
                    if($a -eq "") { 
                        $a+=$_;
                    }else{
                        $a = $a.TrimEnd(' ');
                        $a+="("+$_;
                    }
                } -End {$a};
        $href = $_.href;
        $issue = "";
        $index = -1;
        $name = $nameAndIssue;
        $nameAndIssue.Split(' ') |
            ForEach-Object -Begin {$i = 0} -Process {
                if ([System.Text.RegularExpressions.Regex]::Match($_, "\d{3}").Success) {
                    $issue = $_;
                    $index = $i;
                } elseif ([System.Text.RegularExpressions.Regex]::Match($_, "\d{2}[(]of").Success) {
                    $issue = $_.Replace('(of', '');
                    $index = $i;
                }
                $i++;
            };
        if ($index -eq -1) {
            $issue = [System.Guid]::NewGuid().ToString();
        }
        if ($index -ne -1) {
            $name = $nameAndIssue.Split(' ')[0..($index - 1)] | ForEach-Object -Begin {$a = "";} -Process {$a+=$_ + " "} -End {$a};
        }
        $name = $name.Trim(' ');
        $speed = $cc/([System.DateTime]::Now - $startTime).TotalMinutes;
        $timeLeft = ($maxPageNum - $cc )/$speed * 60;
        $links = (Invoke-WebRequest $href).links | 
            Where-Object {$_.href -like "*blogspot*"} | 
            ForEach-Object -Process { $_.href; };
        $allComics += @(@{Name=$name;Issue=$issue;NameAndIssue=$nameAndIssue;Href=$href;Links=$links});

        Write-Progress -Activity "Page" -PercentComplete ([System.Int32]($cc*100/$maxPageNum)) -SecondsRemaining ([system.int32]($timeLeft)) -Id 1;
        Write-Progress -Activity "Comic" -PercentComplete ([System.Int32]($downLinks*5)) -Id 2 -ParentId 1;
        $downLinks++;
    };
$allComics;