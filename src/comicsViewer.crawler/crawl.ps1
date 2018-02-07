$allComics = @();
$cc = 0;
$startTime = [System.DateTime]::Now;
1..2095 |
    ForEach-Object {$cc++; "http://viewcomic.com/page/" + $_ + "/"} | 
    ForEach-Object {Invoke-WebRequest $_} | 
    ForEach-Object {$_.Links} | 
    Where-Object innerhtml -like "*(*)*" |  
    ForEach-Object {
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
        $issue ="";
        $index = -1;
        $name = $nameAndIssue;
        $nameAndIssue.Split(' ') |
            ForEach-Object -Begin {$i = 0} {
                if ([System.Text.RegularExpressions.Regex]::Match($_, "\d{3}").Success) {
                    $issue = $_;
                    $index = $i;
                } elseif ([System.Text.RegularExpressions.Regex]::Match($_, "\d{2}[(]of").Success) {
                    $issue = $_.Replace('(of', '');
                    $index = $i;
                }
                $i++;
            }
        if ($index -eq -1) {
            $issue = [System.Guid]::NewGuid().ToString();
        }
        if ($index -ne -1) {
            $name = $nameAndIssue.Split(' ')[0..($index - 1)] | ForEach-Object -Begin {$a = "";} -Process {$a+=$_ + " "} -End {$a};
        }
        $name = $name.Trim(' ');
        $speed = $cc/([System.DateTime]::Now - $startTime).TotalMinutes;
        $timeLeft = (2095 - $cc )/$speed * 60;
        Write-Progress -Activity "Crawling" -PercentComplete ([System.Int32]($cc/21)) -SecondsRemaining ([system.int32]($timeLeft));
        $allComics += @(@{Name=$name;Issue=$issue;NameAndIssue=$nameAndIssue;Href=$href});
    };
$allComics;